// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// PSIInterp.cs : Interprets a PSI syntax tree
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;
using static PSI.NType;

public class PSIInterpreter : Visitor {
   public void Run (string source) {
      try {
         // Read the source, and pass it to an ILCodeGen
         Console.WriteLine ($"Compiling {source}...");
         var text = File.ReadAllText (source);
         var node = new Parser (new Tokenizer (text)).Parse ();
         node.Accept (this);
      } catch (ParseException pe) {
         // If there are any syntax errors in our Pascal code, we will get
         // a ParseException from the Parser - report that
         Console.WriteLine ();
         pe.Print ();
      } catch (Exception e) {
         // If there are any other exceptions, they usually indicate a bug
         // in our code
         Console.WriteLine ();
         Console.WriteLine (e);
      }
   }

   public override void Visit (NProgram p) => throw new NotImplementedException ();
   public override void Visit (NBlock b) => throw new NotImplementedException ();
   public override void Visit (NDeclarations d) => throw new NotImplementedException ();
   public override void Visit (NConstDecl c) => throw new NotImplementedException ();
   public override void Visit (NVarDecl d) => throw new NotImplementedException ();
   public override void Visit (NFnDecl f) => throw new NotImplementedException ();

   public override void Visit (NCompoundStmt b) => throw new NotImplementedException ();
   public override void Visit (NAssignStmt a) => throw new NotImplementedException ();
   public override void Visit (NBreakStmt b) => throw new NotImplementedException ();
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