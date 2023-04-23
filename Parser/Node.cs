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
public record NDeclarations (NVarDecl[] Vars, NProcFnDecls ProcFns) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

public record NProcFnDecls (NProcDecl[] Procs, NFnDecl[] Fns) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

public record NProcDecl (Token Name, NParams Params, NBlock Block) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

public record NFnDecl (Token Name, NParams Params, NType Type, NBlock Block) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

public record NParams (NVarDecl[] Vars) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// Declares a variable (with a type)
public record NVarDecl (Token Name, NType Type) : Node {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}
#endregion

#region Statements -------------------------------------------------------------
// Base class for various types of statements
public abstract record NStmt : Node { }

// A compound statement (begin { stmts }* end)
public record NCompoundStmt (NStmt[] Stmts) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A Write or WriteLn statement (NewLine differentiates between the two)
public record NWriteStmt (bool NewLine, NExpr[] Exprs) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A Read statement
public record NReadStmt (Token[] Idents) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// An assignment statement
public record NAssignStmt (Token Name, NExpr Expr) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A call statement
public record NCallStmt (NFnCall FnCall) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A While statement
public record NWhileStmt (NExpr Exp, NStmt Stmt) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// An if statement
public record NIfStmt (NExpr Exp, NStmt Stmt) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// An else statement
public record NElseStmt (NStmt Stmt) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A Repeat statement
public record NRepeatStmt (NStmt[] Stmts, NExpr Exp) : NStmt {
   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A For statement
public record NForStmt (Token Name, NExpr StartExp, bool IsTo, NExpr EndExp, NStmt Stmt) : NStmt {
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
