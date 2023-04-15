namespace PSI;

// Base class for all syntax-tree Nodes
public abstract class Node {
}

// The data-type at any NExpr node
public enum NType { Unknown, Int, Real, Boolean, String, Char, Error };

// Base class for all expression nodes
public abstract class NExpr : Node {
   public NType Type { get; set; }
   public abstract T Accept<T> (Visitor<T> visitor);
}

// Represents a binary operation node
public class NBinary : NExpr {
   public NBinary (NExpr left, Token op, NExpr right) => (Left, Op, Right) = (left, op, right);
   public NExpr Left { get; }
   public Token Op { get; }
   public NExpr Right { get; }

   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// Represents a unary operation node
public class NUnary : NExpr {
   public NUnary (Token op, NExpr expr) => (Op, Expr) = (op, expr);
   public Token Op { get; }
   public NExpr Expr { get; }

   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// An identifier node
public class NIdentifier : NExpr {
   public NIdentifier (Token name) => Name = name;
   public Token Name { get; }

   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A number or string literal node
public class NLiteral : NExpr {
   public NLiteral (Token value) => Value = value;
   public Token Value { get; }

   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A function call node
public class NFnCall : NExpr {
   public NFnCall (Token name, NExpr[] pars) => (Name, Params) = (name, pars);
   public Token Name { get; }
   public NExpr[] Params { get; }

   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}
