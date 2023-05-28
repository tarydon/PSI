// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// ILCodeGen.cs : Compiles a PSI parse tree to IL
// ─────────────────────────────────────────────────────────────────────────────
using System.Text;
namespace PSI;

public class ILCodeGen : Visitor {
   public readonly StringBuilder S = new ();

   public override void Visit (NProgram p) {
      S.Append (".assembly extern System.Runtime { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }\n");
      S.Append (".assembly extern System.Console { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }\n");
      S.Append ($".assembly {p.Name} {{ .ver 0:0:0:0 }}\n\n");
      S.Append (".class Program {\n");
      Visit (p.Block);
      S.Append ("}\n");
   }

   public override void Visit (NBlock b) {
      S.Append ("  .method static void Main () {\n    .entrypoint\n");
      S.Append ("    ret\n  }\n");
   }

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
}