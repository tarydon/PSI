// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Typer.cs ~ Implements ExprTyper, ExprCaster visitors
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;
using static Token.E;
using static NType;

/*
// -----------------------------------------------------------------------------
// ExprTyper assigns a type for each node in an expression tree
public class ExprTyper : Visitor<NType> {
   public ExprTyper (Dictionary<string, NType> types) => mTypes = types;
   Dictionary<string, NType> mTypes;

   public override NType Visit (NLiteral t) 
      => t.Type = t.Value.Kind switch {
         L_INTEGER => Int, L_REAL => Real, L_STRING => String, 
         L_BOOLEAN => Bool, L_CHAR => Char, _ => Error
      };

   public override NType Visit (NIdentifier d)
      => d.Type = mTypes[d.Name.Text];

   public override NType Visit (NUnary u)
      => u.Type = u.Expr.Accept (this);

   public override NType Visit (NBinary bi) {
      NType a = bi.Left.Accept (this), b = bi.Right.Accept (this);
      return bi.Type = (bi.Op.Kind, a, b) switch {
         (ADD or SUB or MUL or DIV, Int or Real, Int or Real) when a == b => a,
         (ADD or SUB or MUL or DIV, Int, Real) => Real,
         (ADD or SUB or MUL or DIV, Real, Int) => Real,
         (MOD, Int, Int) => Int,
         (ADD, String, _) => String,
         (ADD, _, String) => String,
         (LT or LEQ or GT or GEQ, Int or Real, Int or Real) => Bool,
         (LT or LEQ or GT or GEQ, Int or Real or String or Char, Int or Real or String or Char) when a == b => Bool,
         (EQ or NEQ, _, _) when a == b => Bool,
         (EQ or NEQ, Int or Real, Int or Real) => Bool,
         (AND or OR, Int or Bool, Int or Bool) when a == b => a,
         _ => Error
      };
   }

   public override NType Visit (NFnCall f) {
      f.Params.ForEach (a => a.Accept (this));
      return f.Type = mTypes[f.Name.Text];
   }
}*/