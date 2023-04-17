﻿namespace PSI;

// Base class for all syntax-tree Nodes
public abstract class Node {
}

// The data-type at any NExpr node
public enum NType { Unknown, Int, Real, Boolean, String, Char, Error, NoCast };

// Base class for all expression nodes
public abstract class NExpr : Node {
   public NType Type { get; set; }
   public abstract T Accept<T> (Visitor<T> visitor);
}

public class NFnCall : NExpr {
   public NFnCall (Token name, NExpr[] pars) => (Name, Params) = (name, pars);
   public Token Name { get; }
   public NExpr[] Params { get; }

   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// Represents a binary operation node
public class NBinary : NExpr {
   public NBinary (NExpr left, Token op, NExpr right) => (Left, Op, Right) = (left, op, right);
   public NExpr Left { get; set; }
   public Token Op { get; }
   public NExpr Right { get; set; }

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

// The typecast expression.
public class NCast : NExpr {
   public NCast (NExpr expr, NType type) => (Expr, Type) = (expr, type);
   public NExpr Expr { get; }
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// The ExprVisitor interface
public abstract class Visitor<T> {
   public abstract T Visit (NLiteral literal);
   public abstract T Visit (NIdentifier identifier);
   public abstract T Visit (NCast cast);
   public abstract T Visit (NUnary unary);
   public abstract T Visit (NBinary binary);
   public abstract T Visit (NFnCall fn);
}
