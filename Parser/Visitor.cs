// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Visitor.cs : Implements Visitor to traverse the syntax tree
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;

// The Visitor interface (to visit all nodes)
public abstract class Visitor<T> {
   public abstract T Visit (NLiteral t);
   public abstract T Visit (NIdentifier d);
   public abstract T Visit (NUnary u);
   public abstract T Visit (NBinary b);
   public abstract T Visit (NFnCall f);
}
