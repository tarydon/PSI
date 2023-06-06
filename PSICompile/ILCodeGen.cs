// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// ILCodeGen.cs : Compiles a PSI parse tree to IL
// ─────────────────────────────────────────────────────────────────────────────
using System.Text;
namespace PSI;
using static NType;

public class ILCodeGen : Visitor {
   // Generated code is gathered heres
   public readonly StringBuilder S = new ();

   public override void Visit (NProgram p) {
      Out (".assembly extern System.Runtime { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }");
      Out (".assembly extern System.Console { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }");
      Out (".assembly extern PSILib { .ver 1:0:0:0 }");
      Out ($".assembly {p.Name} {{ .ver 0:0:0:0 }}\n");

      Out (".class Program {");
      mSymbols = new SymTable { Parent = mSymbols };
      p.Block.Declarations.Accept (this);
      Out ("  .method static void Main () {");
      Out ("    .entrypoint");
      p.Block.Body.Accept (this);
      mSymbols = mSymbols.Parent;
      Out ("    ret");
      Out ("  }");
      Out ("}");
   }
   SymTable mSymbols = SymTable.Root;

   public override void Visit (NBlock b) { 
      b.Declarations.Accept (this);
      b.Body.Accept (this);
   }

   public override void Visit (NDeclarations d) {
      Visit (d.Consts); Visit (d.Vars); Visit (d.Funcs);
   }

   public override void Visit (NConstDecl c) {
      mSymbols.Add (c);
   }

   public override void Visit (NVarDecl v) {
      mSymbols.Add (v);
      if (v.Local) Out ($"    .locals init ({TMap[v.Type]} {v.Name})");
      else Out ($"    .field static {TMap[v.Type]} {v.Name}");
   }

   public override void Visit (NFnDecl f) {
      mSymbols.Add (f);
      mSymbols = new SymTable { Parent = mSymbols };
      f.Params.ForEach (v => { v.Argument = true; mSymbols.Add (v); });
      var args = f.Params.Select (v => $"{TMap[v.Type]} {v.Name}").ToCSV ();
      Out ($"  .method static {TMap[f.Return]} {f.Name} ({args}) {{");
      if (f.Return != Void) Out ($"    .locals init ( {TMap[f.Return]} {f.Name} )");
      if (f.Block != null) {
         f.Block.Declarations.Vars.ForEach (x => x.Local = true);
         f.Block.Accept (this);
      }
      mSymbols = mSymbols.Parent;
      if (f.Return != Void) Out ($"    ldloc {f.Name.Text}");
      Out ("    ret");
      Out ("  }");
   }

   public override void Visit (NCompoundStmt b) =>
      Visit (b.Stmts);

   public override void Visit (NAssignStmt a) {
      a.Expr.Accept (this);
      StoreVar (a.Name);
   }

   void StoreVar (Token name) {
      var d = mSymbols.Find (name)!;
      switch (d) {
         case NFnDecl fn:
            Out ($"    stloc {fn.Name}");
            break;
         case NVarDecl vd:
            var type = TMap[vd.Type];
            if (vd.Local) Out ($"    stloc {vd.Name}");
            else if (vd.Argument) Out ($"    starg {vd.Name}");
            else Out ($"    stsfld {type} Program::{vd.Name}");
            break;
      } 
   }

   void LoadVar (Token name) {
      var vd = (NVarDecl)mSymbols.Find (name)!;
      var type = TMap[vd.Type];
      if (vd.Local) Out ($"    ldloc {vd.Name}");
      else if (vd.Argument) Out ($"    ldarg {vd.Name}");
      else Out ($"    ldsfld {type} Program::{vd.Name}");
   }

   public override void Visit (NWriteStmt w) {
      foreach (var e in w.Exprs) {
         e.Accept (this);
         Out ($"    call void [System.Console]System.Console::Write ({TMap[e.Type]})");
      }
      if (w.NewLine) Out ("    call void [System.Console]System.Console::WriteLine ()");
   }

   public override void Visit (NIfStmt f) {
      string labl1 = NextLabel (), labl2 = NextLabel ();
      f.Condition.Accept (this);
      Out ($"    brfalse {labl1}");
      f.IfPart.Accept (this);
      Out ($"    br {labl2}");
      Out ($"   {labl1}:");
      f.ElsePart?.Accept (this);
      Out ($"   {labl2}:");
   }

   public override void Visit (NForStmt f) { 
      string labl1 = NextLabel (), labl2 = NextLabel ();
      f.Start.Accept (this);
      StoreVar (f.Var);
      Out ($"    br {labl2}");
      Out ($"   {labl1}:");
      f.Body.Accept (this);
      LoadVar (f.Var);
      Out ($"    ldc.i4.1");
      Out (f.Ascending ? "    add" : "    sub");
      StoreVar (f.Var);
      Out ($"   {labl2}:");
      LoadVar (f.Var);
      f.End.Accept (this);
      Out (f.Ascending ? "    cgt" : "    clt");
      Out ($"    brfalse {labl1}");
   }

   public override void Visit (NReadStmt r) => throw new NotImplementedException ();

   public override void Visit (NWhileStmt w) {
      string lab1 = NextLabel (), lab2 = NextLabel ();
      Out ($"    br {lab2}");
      Out ($"  {lab1}:");
      w.Body.Accept (this);
      Out ($"  {lab2}:");
      w.Condition.Accept (this);
      Out ($"    brtrue {lab1}");
   }

   public override void Visit (NRepeatStmt r) {
      string lab = NextLabel ();
      Out ($"  {lab}:");
      Visit (r.Stmts);
      r.Condition.Accept (this);
      Out ($"    brfalse {lab}");
   }
   string NextLabel () => $"IL_{++mLabel:D4}";
   int mLabel;


   public override void Visit (NCallStmt c) => CallFunction (c.Name, c.Params);

   public override void Visit (NLiteral t) {
      var v = t.Value;
      Out (t.Type switch {
         String => $"    ldstr \"{v.Text}\"",
         Integer => $"    ldc.i4 {v.Text}", 
         Real => $"    ldc.r8 {v.Text}", 
         Bool => $"    ldc.i4 {BoolToInt (v)}",
         Char => $"    ldc.i4 {(int)v.Text[0]}",
         _ => throw new NotImplementedException (),
      });
   }

   public override void Visit (NIdentifier d) {
      switch (mSymbols.Find (d.Name)) {
         case NConstDecl cd: Visit (cd.Value); break;
         case NVarDecl vd:
            var type = TMap[vd.Type];
            if (vd.Local) Out ($"    ldloc {vd.Name}");
            else if (vd.Argument) Out ($"    ldarg {vd.Name}");
            else if (vd.StdLib) {
               Out ($"    call {type} [PSILib]PSILib.Lib::get_{vd.Name} ()");
            } else Out ($"    ldsfld {type} Program::{vd.Name}");
            break;
         default: throw new NotImplementedException ();
      }
   }

   public override void Visit (NUnary u) {
      u.Expr.Accept (this);
      string op = u.Op.Kind.ToString ().ToLower ();
      op = op switch { "sub" => "neg", "not" => "ldc.i4.0\n    ceq", _ => op };
      Out ($"    {op}");
   }

   public override void Visit (NBinary b) {
      b.Left.Accept (this); b.Right.Accept (this);
      if (b.Left.Type == String) 
         Out ("    call string [System.Runtime]System.String::Concat (string, string)");
      else {
         string op = b.Op.Kind.ToString ().ToLower ();
         op = op switch { 
            "mod" => "rem", "eq" => "ceq", "lt" => "clt", "gt" => "cgt", 
            "leq" => "cgt\n    ldc.i4.0\n    ceq",
            "geq" => "clt\n    ldc.i4.0\n    ceq",
            "neq" => "ceq\n    ldc.i4.0\n    ceq",
            _ => op 
         };
         Out ($"    {op}");
      }
   }

   public override void Visit (NFnCall f) => CallFunction (f.Name, f.Params);

   public override void Visit (NTypeCast t) {
      t.Expr.Accept (this);
      Out ((t.Expr.Type, t.Type) switch {
         (Integer, Real) => "    conv.r8",
         (Integer, String) => "   call string [PSILib]PSILib.Helper::CIntStr (int32)",
         _ => throw new NotImplementedException ()
      });
   }

   // Helpers ......................................
   // Append a line to output (followed by a \n newline)
   void CallFunction (Token name, NExpr[] args) {
      Visit (args);
      var fn = (NFnDecl)mSymbols.Find (name)!;
      string fullname = fn.StdLib ? $"[PSILib]PSILib.Lib::{fn.Name.Text}" : $"Program::{fn.Name.Text}";
      string sign = fn.Params.Select (x => TMap[x.Type]).ToCSV ();
      Out ($"    call {TMap[fn.Return]} {fullname} ({sign})");
   }

   void Out (string s) => S.Append (s).Append ('\n');

   // Append text to output (continuing on the same line)
   void OutC (string s) => S.Append (s);

   // Call Accept on a sequence of nodes
   void Visit (IEnumerable<Node> nodes) {
      foreach (var node in nodes) node.Accept (this);
   }

   int BoolToInt (Token token)
      => token.Text.EqualsIC ("TRUE") ? 1 : 0;

   // Dictionary that maps PSI.NType to .Net type names
   static Dictionary<NType, string> TMap = new () {
      [NType.String] = "string", [NType.Integer] = "int32", [NType.Real] = "float64",
      [NType.Bool] = "bool", [NType.Char] = "char", [NType.Void] = "void",
   };
}