// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// ILCodeGen.cs : Compiles a PSI parse tree to IL
// ─────────────────────────────────────────────────────────────────────────────
using System.Text;
namespace PSI;

public class ILCodeGen : Visitor {
   public readonly StringBuilder S = new ();

   public override void Visit (NProgram p) {
      Out (".assembly extern System.Runtime { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }");
      Out (".assembly extern System.Console { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }");
      Out (".assembly extern PSILib { .ver 1:0:0:0 }");
      Out ($".assembly {p.Name} {{ .ver 0:0:0:0 }}\n");

      Out (".class Program {");
      mSymbols = new SymTable { Parent = mSymbols };
      Visit (p.Block.Declarations);
      Out ("\n  .method static void Main () {\n    .entrypoint");
      Visit (p.Block.Body);
      Out ("    ret\n  }");
      mSymbols = mSymbols.Parent;
      Out ("}");
   }
   SymTable mSymbols = SymTable.Root;

   public override void Visit (NBlock b) => throw new NotImplementedException ();

   public override void Visit (NDeclarations d) {
      Visit (d.Consts);
      bool locals = mSymbols.Local && d.Vars.Any ();
      if (locals) { Out ("    .locals init ("); d.Vars.Last ().Last = true; }
      Visit (d.Vars);
      if (locals) Out ("    )");
      Visit (d.Funcs);
   }

   public override void Visit (NConstDecl c) {
      mSymbols.Add (c);
   }

   public override void Visit (NVarDecl d) {
      mSymbols.Add (d);
      if (d.Local) { 
         OutC ($"      {TypeMap[d.Type]} {d.Name}");
         if (!d.Last) Out (","); else Out ("");
      }
      else Out ($"  .field static {TypeMap[d.Type]} {d.Name}");
   }

   public override void Visit (NFnDecl f) {
      mSymbols.Add (f);
      if (f.Block == null) return;
      var pars = f.Params.Select (a => $"{TypeMap[a.Type]} {a.Name}").ToCSV ();
      Out ($"\n  .method static {TypeMap[f.Return]} {f.Name} ({pars}) {{");

      mSymbols = new SymTable { Parent = mSymbols, Local = true };
      foreach (var p in f.Params) { p.Argument = true; mSymbols.Add (p); }
      // mSymbols.Add (new NVarDecl (f.Name, f.Return));
      Visit (f.Block.Declarations); Visit (f.Block.Body); 
      Out ("    ret\n  }");
      mSymbols = mSymbols.Parent;
   }

   public override void Visit (NCompoundStmt b) {
      Visit (b.Stmts);
   }

   public override void Visit (NAssignStmt a) {
      a.Expr.Accept (this);
      var decl = mSymbols.Find (a.Name);
      if (decl is NFnDecl fd) {
      } else if (decl is NVarDecl vd) {
         var type = TypeMap[vd.Type];
         if (vd.Local) Out ($"    stloc {a.Name}");
         else Out ($"    stsfld {type} Program::{a.Name}");
      } else throw new NotImplementedException ();
   }

   public override void Visit (NWriteStmt w) {
      for (int i = 0; i < w.Exprs.Length; i++) {
         var e = w.Exprs[i]; e.Accept (this);
         string typename = TypeMap[e.Type];
         string method = i == w.Exprs.Length - 1 && w.NewLine ? "WriteLine" : "Write";
         Out ($"    call void [System.Console]System.Console::{method} ({typename})");
      }
   }

   public override void Visit (NIfStmt f) {
      f.Condition.Accept (this);
      string lab1 = NextLabel ();
      Out ($"    brfalse {lab1}");
      f.IfPart.Accept (this);
      if (f.ElsePart != null) {
         string lab2 = NextLabel ();
         Out ($"    br {lab2}");
         Out ($"  {lab1}:");
         f.ElsePart.Accept (this);
         Out ($"  {lab2}:");
      } else
         Out ($"  {lab1}:");
   }
   string NextLabel () => $"IL_{++mLabel:D4}";
   int mLabel;

   public override void Visit (NForStmt f) => throw new NotImplementedException ();
   public override void Visit (NReadStmt r) => throw new NotImplementedException ();
   public override void Visit (NWhileStmt w) => throw new NotImplementedException ();
   public override void Visit (NRepeatStmt r) => throw new NotImplementedException ();
   public override void Visit (NCallStmt c) => throw new NotImplementedException ();

   public override void Visit (NLiteral t) {
      var text = t.Value.Text;
      Out (t.Type switch {
         NType.String => $"    ldstr \"{text}\"",
         NType.Integer => $"    ldc.i4 {text}",
         NType.Char => $"    ldc.i4 {(int)text[0]}",
         NType.Real => $"    ldc.r8 {text}",
         NType.Bool => $"    ldc.i4 {BoolToInt (text)}",
         _ => throw new NotImplementedException ()
      });
   }

   public override void Visit (NIdentifier d) {
      switch (mSymbols.Find (d.Name)) {
         case NConstDecl cd: Visit (cd.Value); break;
         case NVarDecl vd:
            if (vd.Argument) Out ($"    ldarg {d.Name}");
            else if (vd.Local) Out ($"    ldloc {d.Name}");
            else Out ($"    ldsfld {TypeMap[vd.Type]} Program::{d.Name}"); 
            break;

         default: throw new NotImplementedException ();
      }
   }

   public override void Visit (NUnary u) {
      u.Expr.Accept (this);
      switch (u.Op.Kind) {
         case Token.E.ADD: break;
         case Token.E.SUB: Out ("    neg"); break;
         case Token.E.NOT:
            if (u.Expr.Type == NType.Bool) Out ("    ldc.i4.0\n    ceq"); 
            else Out ("    not");
            break;
         default: throw new NotImplementedException ();
      }
   }

   public override void Visit (NBinary b) {
      b.Left.Accept (this); b.Right.Accept (this);
      if (b.Type == NType.String)
         Out ("    call string [System.Runtime]System.String::Concat(string, string)");
      else {
         string op = b.Op.Kind.ToString ().ToLower (); ;
         op = op switch { 
            "mod" => "rem", "lt" => "clt", "gt" => "cgt", "eq" => "ceq",
            "leq" => "cgt|ldc.i4.0|ceq", "neq" => "ceq|ldc.i4.0|ceq", "geq" => "clt|ldc.i4.0|ceq",
            _ => op
         };
         foreach (var w in op.Split ('|'))
            Out ($"    {w}");
      }
   }

   public override void Visit (NFnCall f) {
      Visit (f.Params);
      var fd = (NFnDecl)mSymbols.Find (f.Name)!;
      var pars = fd.Params.Select (a => TypeMap[a.Type]).ToCSV ();
      Out ($"    call {TypeMap[fd.Return]} Program::{fd.Name} ({pars})");
   }

   public override void Visit (NTypeCast t) {
      t.Expr.Accept (this);
      Out ((t.Expr.Type, t.Type) switch {
         (_, NType.Real) => "    conv.r8",
         (NType.Integer, NType.String) => "    call string [PSILib]PSILib.Helper::CIntStr (int32)",
         _ => throw new NotImplementedException ()
      });
   }

   void Out (string s) => S.Append (s).Append ('\n');
   void OutC (string s) => S.Append (s);

   void Visit (IEnumerable<Node> nodes) {
      foreach (var node in nodes) node.Accept (this);
   }

   static Dictionary<NType, string> TypeMap = new () {
      [NType.String] = "string", [NType.Integer] = "int32", [NType.Real] = "float64",
      [NType.Bool] = "bool", [NType.Char] = "char", [NType.Void] = "void",
   };

   int BoolToInt (string text) => text.EqualsIC ("TRUE") ? 1 : 0;
}