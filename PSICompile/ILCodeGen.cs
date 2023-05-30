// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// ILCodeGen.cs : Compiles a PSI parse tree to IL
// ─────────────────────────────────────────────────────────────────────────────
using System.Text;
namespace PSI;

public class ILCodeGen : Visitor {
   // Generated code is gathered heres
   public readonly StringBuilder S = new ();

   public override void Visit (NProgram p) {
      Out (".assembly extern System.Runtime { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }");
      Out (".assembly extern System.Console { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }");
      Out (".assembly extern PSILib { .ver 1:0:0:0 }");
      Out ($".assembly {p.Name} {{ .ver 0:0:0:0 }}\n");

      Out (".class Program {");
      Out ("  .method static void Main () {");
      Out ("    .entrypoint");
      Out ("    .line 3,3 : 5,10 'P:/TData/Compile/Comp0.pas'");
      Out ("    ldstr \"Hello, World!\"");
      Out ("    call void [System.Console]System.Console::WriteLine (string)");
      Out ("    ret");
      Out ("  }");
      Out ("}");
   }

   public override void Visit (NBlock b) => throw new NotImplementedException ();
   public override void Visit (NDeclarations d) => throw new NotImplementedException ();
   public override void Visit (NConstDecl c) => throw new NotImplementedException ();
   public override void Visit (NVarDecl d) => throw new NotImplementedException ();
   public override void Visit (NFnDecl f) => throw new NotImplementedException ();

   public override void Visit (NCompoundStmt b) => throw new NotImplementedException ();
   public override void Visit (NAssignStmt a) => throw new NotImplementedException ();
   public override void Visit (NWriteStmt w) => throw new NotImplementedException ();
   public override void Visit (NIfStmt f) => throw new NotImplementedException ();
   public override void Visit (NForStmt f) => throw new NotImplementedException ();
   public override void Visit (NReadStmt r) => throw new NotImplementedException ();
   public override void Visit (NWhileStmt w) => throw new NotImplementedException ();
   public override void Visit (NRepeatStmt r) => throw new NotImplementedException ();
   public override void Visit (NCallStmt c) => throw new NotImplementedException ();

   public override void Visit (NLiteral t) => throw new NotImplementedException ();
   public override void Visit (NIdentifier d) => throw new NotImplementedException ();
   public override void Visit (NUnary u) => throw new NotImplementedException ();
   public override void Visit (NBinary b) => throw new NotImplementedException ();
   public override void Visit (NFnCall f) => throw new NotImplementedException ();
   public override void Visit (NTypeCast t) => throw new NotImplementedException ();

   // Helpers ......................................
   // Append a line to output (followed by a \n newline)
   void Out (string s) => S.Append (s).Append ('\n');

   // Append text to output (continuing on the same line)
   void OutC (string s) => S.Append (s);

   // Call Accept on a sequence of nodes
   void Visit (IEnumerable<Node> nodes) {
      foreach (var node in nodes) node.Accept (this);
   }

   // Dictionary that maps PSI.NType to .Net type names
   static Dictionary<NType, string> TMap = new () {
      [NType.String] = "string", [NType.Integer] = "int32", [NType.Real] = "float64",
      [NType.Bool] = "bool", [NType.Char] = "char", [NType.Void] = "void",
   };
}