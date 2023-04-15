namespace PSI;

// The ExprVisitor interface
public abstract class Visitor<T> {
   public abstract T Visit (NLiteral literal);
   public abstract T Visit (NIdentifier identifier);
   public abstract T Visit (NUnary unary);
   public abstract T Visit (NBinary binary);
   public abstract T Visit (NFnCall fn);
}
