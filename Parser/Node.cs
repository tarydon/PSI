// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Node.cs ~ All the syntax tree Nodes
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;

// Base class for all program nodes
public abstract record Node {
   public abstract T Accept<T> (Visitor<T> visitor);
}

#region Main, Declarations -----------------------------------------------------
// The Program node (top node)
public record NProgram (Token Name, NBlock Block) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A block contains declarations and a body
public record NBlock (NDeclarations Decls, NCompoundStmt Body) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// The declarations section precedes the body of every block
public record NDeclarations (NDecl[] Decls) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (Decls);
}

// Base class for various types of declarations (label, variable, function)
public abstract record NDecl : Node { }

// Declares a label 
public record NLabelDecl (Token Name) : NDecl {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// Declares a variable (with a type)
public record NVarDecl (Token Name, NType Type) : NDecl {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// Declares a function (or procedure) 
public record NFnDecl (Token Name, NVarDecl[] Params, NType Return, NBlock Block) : NDecl {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}
#endregion

#region Statements -------------------------------------------------------------
// Base class for various types of statements
public abstract record NStmt : Node { }

// A compound statement (begin { stmts }* end)
public record NCompoundStmt (NStmt[] Stmts) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (Stmts);
}

// A Write or WriteLn statement (NewLine differentiates between the two)
public record NWriteStmt (bool NewLine, NExpr[] Exprs) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// An assignment statement
public record NAssignStmt (Token Name, NExpr Expr) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}
#endregion

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
