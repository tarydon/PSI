// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// PSIPrint.cs ~ Prints a PSI syntax tree in Pascal format
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;

public class PSIPrint : Visitor<StringBuilder> {
   public override StringBuilder Visit (NProgram p) => throw new NotImplementedException ();
   public override StringBuilder Visit (NBlock b) => throw new NotImplementedException ();
   public override StringBuilder Visit (NLabelDecl l) => throw new NotImplementedException ();
   public override StringBuilder Visit (NVarDecl d) => throw new NotImplementedException ();
   public override StringBuilder Visit (NFnDecl f) => throw new NotImplementedException ();
   public override StringBuilder Visit (NAssignStmt a) => throw new NotImplementedException ();
   public override StringBuilder Visit (NWriteStmt w) => throw new NotImplementedException ();
   public override StringBuilder Visit (NLiteral t) => throw new NotImplementedException ();
   public override StringBuilder Visit (NIdentifier d) => throw new NotImplementedException ();
   public override StringBuilder Visit (NUnary u) => throw new NotImplementedException ();
   public override StringBuilder Visit (NBinary b) => throw new NotImplementedException ();
   public override StringBuilder Visit (NFnCall f) => throw new NotImplementedException ();
}