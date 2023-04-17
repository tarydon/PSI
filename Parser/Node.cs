// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Node.cs ~ All the syntax tree Nodes
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;

// Base class for all program nodes
public abstract record Node {
   public abstract T Accept<T> (Visitor<T> visitor);
}

#region Expression nodes -------------------------------------------------------
// Base class for expression nodes
public abstract record NExpr : Node {
   public NType Type { get; set; }     // The type of this expression
}

// A Literal (string / real / integer /  ...)
public record NLiteral (Token Value) : NExpr {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// An identifier (type depends on symbol-table lookup)
public record NIdentifier (Token Name) : NExpr {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// Unary operator expression
public record NUnary (Token Op, NExpr Expr) : NExpr {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// Binary operator expression 
public record NBinary (NExpr Left, Token Op, NExpr Right) : NExpr {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A function-call node in an expression
public record NFnCall (Token Name, NExpr[] Params) : NExpr {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}
#endregion
