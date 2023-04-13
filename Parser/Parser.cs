namespace PSI;
using static Token.E;

public class Parser {
   // Interface -------------------------------------------
   public Parser (Tokenizer tokenizer) 
      => mToken = mPrevious = (mTokenizer = tokenizer).Next ();

   public NExpr Parse () {
      var node = Expression ();
      if (mToken.Kind != EOF) throw new Exception ($"Unexpected {mToken}");
      return node;
   }

   // Implementation --------------------------------------
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

   // primary = IDENTIFIER | INTEGER | REAL | STRING | "(" expression ")" | "not" primary .
   NExpr Primary () {
      if (Match (IDENT)) {
         if (Peek (OPEN)) return new NFnCall (Prev, ArgsList ());
         else return new NIdentifier (Prev); 
      }
      if (Match (INTEGER, REAL, BOOLEAN, CHAR, STRING)) return new NLiteral (Prev);
      if (Match (NOT)) return new NUnary (Prev, Primary ());
      Expect (OPEN, "Expecting identifier or literal");
      var expr = Expression ();
      Expect (CLOSE, "Expecting ')'");
      return expr;
   }

   // Helpers ---------------------------------------------
   // Expect to find a particular token
   void Expect (Token.E kind, string message) {
      if (!Match (kind)) throw new Exception (message);
   }
   // Parse argument list for a function call
   NExpr[] ArgsList () {
      Expect (OPEN, "Expecting identifier or literal");
      List<NExpr> args = new (); 
      if (!Peek (CLOSE)) args.Add (Expression ());
      while (!Peek (CLOSE)) {
         Expect (COMMA, "Expecting ','");
         args.Add (Expression ());       
      }
      Expect (CLOSE, "Expecting ')'");
      return args.ToArray ();
   }

   // Match and consume a token on match
   bool Match (params Token.E[] kinds) {
      if (kinds.Contains (mToken.Kind)) {
         mPrevious = mToken;
         mToken = mTokenizer.Next ();
         return true;
      }
      return false;
   }

   // Like match, but does not consume the token
   bool Peek (params Token.E[] kinds)
      => kinds.Contains (mToken.Kind);

   // The 'previous' token we found
   Token Prev => mPrevious;

   // Private data ---------------------------------------
   Token mToken, mPrevious;
   Tokenizer mTokenizer;
}