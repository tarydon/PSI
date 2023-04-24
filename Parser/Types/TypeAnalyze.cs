// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// TypeAnalyze.cs ~ Type checking, type coercion
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;
using static NType;
using static Token.E;

class TypeAnalyze : Visitor<NType> {
   #region Declarations 
   public override NType Visit (NProgram p) => throw new NotImplementedException ();
   public override NType Visit (NBlock b) => throw new NotImplementedException ();
   public override NType Visit (NDeclarations d) => throw new NotImplementedException ();
   public override NType Visit (NVarDecl d) => throw new NotImplementedException ();
   public override NType Visit (NFnDecl f) => throw new NotImplementedException ();
   public override NType Visit (NCompoundStmt b) => throw new NotImplementedException ();
   public override NType Visit (NAssignStmt a) => throw new NotImplementedException ();
   public override NType Visit (NWriteStmt w) => throw new NotImplementedException ();
   public override NType Visit (NIfStmt f) => throw new NotImplementedException ();
   public override NType Visit (NForStmt f) => throw new NotImplementedException ();
   public override NType Visit (NReadStmt r) => throw new NotImplementedException ();
   public override NType Visit (NWhileStmt w) => throw new NotImplementedException ();
   public override NType Visit (NRepeatStmt r) => throw new NotImplementedException ();
   public override NType Visit (NCallStmt c) => throw new NotImplementedException ();
   #endregion

   #region Expression 
   public override NType Visit (NLiteral t) {
      t.Type = t.Value.Kind switch {
         INTEGER => Int, REAL => Real, BOOLEAN => Bool, STRING => String,
         CHAR => Char, _ => Error,
      };
      return t.Type;
   }

   public override NType Visit (NUnary u) 
      => u.Expr.Accept (this);

   public override NType Visit (NBinary bin) {
      NType a = bin.Left.Accept (this), b = bin.Right.Accept (this);
      return bin.Type = (bin.Op.Kind, a, b) switch {
         (ADD or SUB or MUL or DIV, Int or Real, Int or Real) when a == b => a,
         (ADD or SUB or MUL or DIV, Int or Real, Int or Real) => Real,
         (MOD, Int, Int) => Int,
         (ADD, String, _) => String, 
         (ADD, _, String) => String,
         (LT or LEQ or GT or GEQ, Int or Real, Int or Real) => Bool,
         (LT or LEQ or GT or GEQ, Int or Real or String or Char, Int or Real or String or Char) when a == b => Bool,
         (EQ or NEQ, _, _) when a == b => Bool,
         (EQ or NEQ, Int or Real, Int or Real) => Bool,
         (AND or OR, Int or Bool, Int or Bool) when a == b => a,
         _ => Error,
      };
   }

   public override NType Visit (NIdentifier d) => throw new NotImplementedException ();

   public override NType Visit (NFnCall f) => throw new NotImplementedException ();
   #endregion
}
