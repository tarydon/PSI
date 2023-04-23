// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Parser.cs ~ Recursive descent parser for Pascal Grammar
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;
using static Token.E;
using static NType;
using System.Collections.Generic;

public class Parser {
   // Interface -------------------------------------------
   public Parser (Tokenizer tokenizer)
      => mToken = mPrevPrev = mPrevious = (mTokenizer = tokenizer).Next ();

   public NProgram Parse () {
      var node = Program ();
      if (mToken.Kind != EOF) Unexpected ();
      return node;
   }

   #region Declarations ------------------------------------
   // program = "program" IDENT ";" block "." .
   NProgram Program () {
      Expect (PROGRAM); var name = Expect (IDENT); Expect (SEMI);
      var block = Block (); Expect (PERIOD);
      return new (name, block);
   }

   // block = declarations compound-stmt .
   NBlock Block ()
      => new (Declarations (), CompoundStmt ());

   // declarations = [var-decls] [procfn-decls] .
   NDeclarations Declarations () {
      List<NVarDecl> vars = new ();
      if (Match (VAR)) 
         do { vars.AddRange (VarDecls ()); Expect (SEMI); } while (Peek (IDENT));
      return new (vars.ToArray (), ProcFnDecls ());
   }

   // ident-list = IDENT { "," IDENT }
   Token[] IdentList () {
      List<Token> names = new ();
      do { names.Add (Expect (IDENT)); } while (Match (COMMA));
      return names.ToArray (); 
   }

   // procfn-decls = { proc-decl | func-decl }
   NProcFnDecls ProcFnDecls () {
      List<NProcDecl> procDecls = new (); List<NFnDecl> fnDecls = new List<NFnDecl> ();
      for (; ;) {
         if (Peek (PROCEDURE)) procDecls.Add (ProcDecl ());
         else if (Peek (FUNCTION)) fnDecls.Add (FuncDecl ());
         else break;
      }
      return new (procDecls.ToArray (), fnDecls.ToArray ());
   }

   // var-decl = ident-list ":" type
   NVarDecl[] VarDecls () {
      var names = IdentList (); Expect (COLON); var type = Type ();
      return names.Select (a => new NVarDecl (a, type)).ToArray ();
   }

   // proc-decl = "procedure" IDENT paramlist; block ";" .
   NProcDecl ProcDecl () {
      Expect (PROCEDURE); var name = Expect (IDENT); var paramsList = ParamList (); Expect (SEMI);
      var block = Block (); Expect (SEMI);
      return new (name, paramsList, block);
   }

   // func-decl = "function" IDENT paramlist ":" type; block ";" .
   NFnDecl FuncDecl () {
      Expect (FUNCTION); var name = Expect (IDENT); var paramsList = ParamList (); 
      Expect (COLON); var type = Type (); Expect (SEMI);
      var block = Block (); Expect (SEMI);
      return new (name, paramsList, type, block);
   }

   // paramlist = "(" var-decl { "," var-decl } ")"
   NParams ParamList () {
      List<NVarDecl> vars = new ();
      Expect (OPEN);
      while (!Match (CLOSE)) {
         do { vars.AddRange (VarDecls ()); } while (Match (SEMI));
      }
      return new (vars.ToArray ());
   }

   // type = integer | real | boolean | string | char
   NType Type () {
      var token = Expect (INTEGER, REAL, BOOLEAN, STRING, CHAR);
      return token.Kind switch {
         INTEGER => Int, REAL => Real, BOOLEAN => Bool, 
         STRING => String, _ => Char,
      };
   }
   #endregion
   
   #region Statements ---------------------------------------
   // statement         =  write-stmt | read-stmt | assign-stmt | call-stmt |
   //                      goto-stmt | if-stmt | while-stmt | repeat-stmt |
   //                      compound-stmt | for-stmt | case-stmt
   NStmt Stmt () {
      if (Match (WRITE, WRITELN)) return WriteStmt ();
      if (Match (READ)) return ReadStmt ();
      if (Match (IDENT)) {
         if (Match (ASSIGN)) return AssignStmt ();
         return CallStmt ();
      }
      if (Match (IF)) return IfStmt ();
      if (Match (ELSE)) return ElseStmt ();
      if (Match (WHILE)) return WhileStmt ();
      if (Match (REPEAT)) return RepeatStmt ();
      if (Match (FOR)) return ForStmt ();
      if (Peek (BEGIN)) return CompoundStmt ();
      Unexpected ();
      return null!;
   }

   // write-stmt =  ( "writeln" | "write" ) arglist .
   NWriteStmt WriteStmt () 
      => new (Prev.Kind == WRITELN, ArgList ());

   // read-stmt = "read" "(" identlist ")" .
   NReadStmt ReadStmt () {
      Expect (OPEN); var idents = IdentList (); Expect (CLOSE);
      return new (idents);
   }

   // assign-stmt = IDENT ":=" expr .
   NAssignStmt AssignStmt ()
   => new (PrevPrev, Expression ());

   // call-stmt = IDENT arglist.
   NCallStmt CallStmt () => new (new (Prev, ArgList ()));

   // if-stmt = "if" expression "then" statement["else" statement] .
   NIfStmt IfStmt () {
      var exp = Expression ();
      Expect (THEN);
      return new (exp, Stmt ());
   }

   NElseStmt ElseStmt () => new (Stmt ());

   // while-stmt = "while" expression "do" statement.
   NWhileStmt WhileStmt () {
      var exp = Expression () ;
      Expect (DO); var stmt = Stmt ();
      return new (exp, stmt);
   }

   // repeat-stmt = "repeat" statement { ";" statement } "until" expression.
   NRepeatStmt RepeatStmt () {
      var stmt = Stmt (); Expect (SEMI);
      List<NStmt> stmts = new () { stmt };
      while (!Match (UNTIL)) { stmts.Add (Stmt ()); Match (SEMI); }
      return new (stmts.ToArray (), Expression ());

   }

   // compound-stmt = "begin" [ statement { ";" statement } ] "end" .
   NCompoundStmt CompoundStmt () {
      Expect (BEGIN);
      List<NStmt> stmts = new ();
      while (!Match (END)) { stmts.Add (Stmt ()); Match (SEMI); }
      return new (stmts.ToArray ());
   }

   // for-stmt = "for" IDENT ":=" expression ( "to" | "downto" ) expression "do" statement .
   NForStmt ForStmt () {
      var ident = Expect (IDENT); Expect (ASSIGN); var exp = Expression ();
      var ident2 = Expect (TO, DOWNTO); var exp2 = Expression ();
      Expect (DO);
      return new (ident, exp, ident2.Kind == TO, exp2, Stmt ());
   }

   #endregion

   #region Expression --------------------------------------
   // expression = equality .
   NExpr Expression () 
      => Equality ();

   // equality = equality = comparison [ ("=" | "<>") comparison ] .
   NExpr Equality () {
      var expr = Comparison ();
      if (Match (EQ, NEQ)) 
         expr = new NBinary (expr, Prev, Comparison ());
      return expr;
   }

   // comparison = term [ ("<" | "<=" | ">" | ">=") term ] .
   NExpr Comparison () {
      var expr = Term ();
      if (Match (LT, LEQ, GT, GEQ))
         expr = new NBinary (expr, Prev, Term ());
      return expr;
   }

   // term = factor { ( "+" | "-" | "or" ) factor } .
   NExpr Term () {
      var expr = Factor ();
      while  (Match (ADD, SUB, OR)) 
         expr = new NBinary (expr, Prev, Factor ());
      return expr;
   }

   // factor = unary { ( "*" | "/" | "and" | "mod" ) unary } .
   NExpr Factor () {
      var expr = Unary ();
      while (Match (MUL, DIV, AND, MOD)) 
         expr = new NBinary (expr, Prev, Unary ());
      return expr;
   }

   // unary = ( "-" | "+" ) unary | primary .
   NExpr Unary () {
      if (Match (ADD, SUB))
         return new NUnary (Prev, Unary ());
      return Primary ();
   }

   // primary = IDENTIFIER | INTEGER | REAL | STRING | "(" expression ")" | "not" primary | IDENTIFIER arglist .
   NExpr Primary () {
      if (Match (IDENT)) {
         if (Peek (OPEN)) return new NFnCall (Prev, ArgList ());
         return new NIdentifier (Prev);
      }
      if (Match (L_INTEGER, L_REAL, L_BOOLEAN, L_CHAR, L_STRING)) return new NLiteral (Prev);
      if (Match (NOT)) return new NUnary (Prev, Primary ());
      Expect (OPEN, "Expecting identifier or literal");
      var expr = Expression ();
      Expect (CLOSE);
      return expr;
   }

   // arglist = "(" [ expression { , expression } ] ")"
   NExpr[] ArgList () {
      List<NExpr> args = new ();
      Expect (OPEN);
      if (!Peek (CLOSE)) args.Add (Expression ());
      while (Match (COMMA)) args.Add (Expression ());
      Expect (CLOSE);
      return args.ToArray ();
   }
   #endregion

   #region Helpers -----------------------------------------
   // Expect to find a particular token
   Token Expect (Token.E kind, string message) {
      if (!Match (kind)) Throw (message);
      return mPrevious;
   }

   Token Expect (params Token.E[] kinds) {
      if (!Match (kinds)) 
         Throw ($"Expecting {string.Join (" or ", kinds)}");
      return mPrevious;
   }

   // Like Match, but does not consume the token
   bool Peek (params Token.E[] kinds)
      => kinds.Contains (mToken.Kind);

   // Match and consume a token on match
   bool Match (params Token.E[] kinds) {
      if (kinds.Contains (mToken.Kind)) {
         mPrevPrev = mPrevious; mPrevious = mToken; 
         mToken = mTokenizer.Next ();
         return true;
      }
      return false;
   }

   [DoesNotReturn]
   void Throw (string message) {
      throw new ParseException (mTokenizer.FileName, mTokenizer.Lines, mToken.Line, mToken.Column, message);
   }

   [DoesNotReturn]
   void Unexpected () {
      string message = $"Unexpected {mToken}";
      if (mToken.Kind == ERROR) message = mToken.Text;
      Throw (message);
   }

   // The 'previous' two tokens we've seen
   Token Prev => mPrevious;
   Token PrevPrev => mPrevPrev;

   Token mToken, mPrevious, mPrevPrev;
   readonly Tokenizer mTokenizer;
   #endregion 
}