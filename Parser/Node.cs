// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Node.cs ~ All the syntax tree Nodes
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;

// Base class for all program nodes
public abstract record Node {
   public abstract T Accept<T> (Visitor<T> visitor);
   public abstract void Accept (Visitor visitor);
}

#region Main, Declarations -----------------------------------------------------
// The Program node (top node)
public record NProgram (Token Name, NBlock Block) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// A block contains declarations and a body
public record NBlock (NDeclarations Declarations, NCompoundStmt Body) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// The declarations section precedes the body of every block
public record NDeclarations (NConstDecl[] Consts, NVarDecl[] Vars, NFnDecl[] Funcs) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// A const, variable or function declaration
public abstract record NDecl (Token Name) : Node {
}

// Declares a constant 
public record NConstDecl (Token Name, NLiteral Value) : NDecl (Name) {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// Declares a variable (with a type)
public record NVarDecl (Token Name, NType Type) : NDecl (Name) {
   public bool Assigned { get; set; }   // Has this variable been assigned in this block?
   public bool Local { get; set; }      // Is this a local variable (else, it is Global)
   public bool StdLib { get; set; }     // Is this variable declared in the standard library
   public bool Argument { get; set; }   // Is this a parameter to a function?
   public bool Last { get; set; }       // Is this the last parameter to a function?
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// Declares a function (or procedure) 
public record NFnDecl (Token Name, NVarDecl[] Params, NType Return, NBlock? Block) : NDecl (Name) {
   public bool Assigned { get; set; }
   public bool StdLib { get; set; } 
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}
#endregion

#region Statements -------------------------------------------------------------
// Base class for various types of statements
public abstract record NStmt : Node { }

public record NBreakStmt (Token Token, Token? BreakTo) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// A compound statement (begin { stmts }* end)
public record NCompoundStmt (Token Token, NStmt[] Stmts) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// An if statement (with optional else part)
public record NIfStmt (NExpr Condition, NStmt IfPart, NStmt? ElsePart) : NStmt {
   public NExpr Condition { get; set; } = Condition;
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// A for statement
public record NForStmt (Token Var, NExpr Start, bool Ascending, NExpr End, NStmt Body) : NStmt {
   public NExpr Start { get; set; } = Start;
   public NExpr End { get; set; } = End;
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// A while loop
public record NWhileStmt (NExpr Condition, NStmt Body) : NStmt {
   public NExpr Condition { get; set; } = Condition;
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// A Write or WriteLn statement (NewLine differentiates between the two)
public record NWriteStmt (bool NewLine, NExpr[] Exprs) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// A repeat statement
public record NRepeatStmt (NStmt[] Stmts, NExpr Condition) : NStmt {
   public NExpr Condition { get; set; } = Condition;
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// Read statement
public record NReadStmt (Token[] Vars) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// A call to a procedure
public record NCallStmt (Token Name, NExpr[] Params) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// An assignment statement
public record NAssignStmt (Token Name, NExpr Expr) : NStmt {
   public NExpr Expr { get; set; } = Expr;
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}
#endregion

#region Expression nodes -------------------------------------------------------
// Base class for expression nodes
public abstract record NExpr (Token Token) : Node {
   public NType Type { get; set; }     // The type of this expression
}

// A Literal (string / real / integer /  ...)
public record NLiteral (Token Value) : NExpr (Value) {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// An identifier (type depends on symbol-table lookup)
public record NIdentifier (Token Name) : NExpr (Name) {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// Unary operator expression
public record NUnary (Token Op, NExpr Expr) : NExpr (Op) {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// Binary operator expression 
public record NBinary (NExpr Left, Token Op, NExpr Right) : NExpr (Op) {
   public NExpr Left { get; set; } = Left;
   public NExpr Right { get; set; } = Right;
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

// A function-call node in an expression
public record NFnCall (Token Name, NExpr[] Params) : NExpr (Name) {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}

public record NTypeCast (NExpr Expr) : NExpr (Expr.Token) {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
   public override void Accept (Visitor visitor) => visitor.Visit (this);
}
#endregion
