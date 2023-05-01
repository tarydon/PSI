// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Visitor.cs : Implements Visitor to traverse the syntax tree
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;

// The Visitor interface (to visit all nodes)
public abstract class Visitor<T> {
   public abstract T Visit (NProgram p);
   public abstract T Visit (NBlock b);
   public abstract T Visit (NDeclarations d);
   public abstract T Visit (NConstDecl c);
   public abstract T Visit (NVarDecl d);
   public abstract T Visit (NFnDecl f);

   public abstract T Visit (NCompoundStmt b);
   public abstract T Visit (NAssignStmt a);
   public abstract T Visit (NWriteStmt w);
   public abstract T Visit (NIfStmt f);
   public abstract T Visit (NForStmt f);
   public abstract T Visit (NReadStmt r);
   public abstract T Visit (NWhileStmt w);
   public abstract T Visit (NRepeatStmt r);
   public abstract T Visit (NCallStmt c);

   public abstract T Visit (NLiteral t);
   public abstract T Visit (NIdentifier d);
   public abstract T Visit (NUnary u);
   public abstract T Visit (NBinary b);
   public abstract T Visit (NFnCall f);
   public abstract T Visit (NTypeCast t);
}
