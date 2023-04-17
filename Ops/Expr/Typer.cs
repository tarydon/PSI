namespace PSI.Ops;
using static Token.E;
using static NType;

// Computes the type of an expression, using the Visitor pattern
// This updates the type on each node
public class ExprTyper : Visitor<NType> {
   public ExprTyper (Dictionary<string, NType> symbols) => mSymbols = symbols;
   Dictionary<string, NType> mSymbols;

   public override NType Visit (NLiteral literal) => 
      literal.Type = literal.Value.Kind switch {
         INTEGER => Int, REAL => Real, BOOLEAN => Boolean,
         STRING => String, CHAR => Char, _ => Error
      };

   public override NType Visit (NIdentifier identifier) =>
      identifier.Type = mSymbols[identifier.Name.Text];

   public override NType Visit (NUnary unary)
      => unary.Type = unary.Expr.Accept (this);

   public override NType Visit (NBinary binary) {
      NType a = binary.Left.Accept (this), b = binary.Right.Accept (this);
      return binary.Type = (binary.Op.Kind, a, b) switch {
         (ADD or SUB or MUL or DIV, Int or Real, Int or Real) when a == b => a,
         (ADD or SUB or MUL or DIV, Int, Real) => Real,
         (ADD or SUB or MUL or DIV, Real, Int) => Real,
         (MOD, Int, Int) => Int,
         (ADD, String, _) => String,
         (ADD, _, String) => String,
         (LT or LEQ or GT or GEQ, Int or Real, Int or Real) => Boolean,
         (LT or LEQ or GT or GEQ, Int or Real or String or Char, Int or Real or String or Char) when a == b => Boolean,
         (EQ or NEQ, _, _) when a == b => Boolean,
         (EQ or NEQ, Int or Real, Int or Real) => Boolean,
         (AND or OR, Int or Boolean, Int or Boolean) when a == b => a,
         _ => Error
      };
   }

   public override NType Visit (NFnCall fn) {
      foreach (var param in fn.Params) param.Accept (this);
      // Dummy implementation.
      return fn.Type = fn.Name.Text.ToUpper () switch {
         "LENGTH" or "RANDOM" => Int,
         _ => Real
      };
   }
}