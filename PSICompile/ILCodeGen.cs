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
      Visit (p.Block);
      Out ("}");
   }

   public override void Visit (NBlock b) {
      mSymbols = new SymTable { Parent = mSymbols };
      Visit (b.Declarations);
      Out ("  .method static void Main () {\n    .entrypoint");
      Visit (b.Body);
      Out ("    ret\n  }");
      mSymbols = mSymbols.Parent;
   }
   SymTable mSymbols = SymTable.Root;

   public override void Visit (NDeclarations d) {
      Visit (d.Consts); Visit (d.Vars); Visit (d.Funcs);
   }

   public override void Visit (NConstDecl c) => throw new NotImplementedException ();
   public override void Visit (NVarDecl d) => throw new NotImplementedException ();
   public override void Visit (NFnDecl f) => throw new NotImplementedException ();

   public override void Visit (NCompoundStmt b) {
      Visit (b.Stmts);
   }

   public override void Visit (NAssignStmt a) => throw new NotImplementedException ();

   public override void Visit (NWriteStmt w) {
      for (int i = 0; i < w.Exprs.Length; i++) {
         var e = w.Exprs[i]; e.Accept (this);
         string typename = TypeMap[e.Type];
         string method = i == w.Exprs.Length - 1 && w.NewLine ? "WriteLine" : "Write";
         Out ($"    call void [System.Console]System.Console::{method} ({typename})");
      }
   }

   public override void Visit (NIfStmt f) => throw new NotImplementedException ();
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

   public override void Visit (NIdentifier d) => throw new NotImplementedException ();

   public override void Visit (NUnary u) {
      u.Expr.Accept (this);
      if (u.Op.Kind == Token.E.SUB) Out ("    neg");
   }

   public override void Visit (NBinary b) {
      b.Left.Accept (this); b.Right.Accept (this);
      if (b.Type == NType.String)
         Out ("    call string [System.Runtime]System.String::Concat(string, string)");
      else {
         string op = b.Op.Kind.ToString ().ToLower (); ;
         op = op switch { "mod" => "rem", _ => op };
         Out ($"    {op}");
      }
   }

   public override void Visit (NFnCall f) => throw new NotImplementedException ();

   public override void Visit (NTypeCast t) {
      t.Expr.Accept (this);
      Out ((t.Expr.Type, t.Type) switch {
         (_, NType.Real) => "    conv.r8",
         (NType.Integer, NType.String) => "    call string [PSILib]PSILib.Helper::CIntStr (int32)",
         _ => throw new NotImplementedException ()
      });
   }

   void Out (string s) => S.Append (s).Append ('\n');

   void Visit (IEnumerable<Node> nodes) {
      foreach (var node in nodes) node.Accept (this);
   }

   static Dictionary<NType, string> TypeMap = new () {
      [NType.String] = "string", [NType.Integer] = "int32", [NType.Real] = "float64",
      [NType.Bool] = "bool", [NType.Char] = "char", [NType.Void] = "void",
   };

   int BoolToInt (string text) => text.EqualsIC ("TRUE") ? 1 : 0;
}