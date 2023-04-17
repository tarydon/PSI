namespace PSI.Ops;
using static Token.E;

// An expression evaluator, implementing using the visitor pattern
public class ExprEvaluator : Visitor<double> {
   public ExprEvaluator (Dictionary<string, double> dict) => mDict = dict;
   Dictionary<string, double> mDict;

   public override double Visit (NLiteral literal)
      => double.Parse (literal.Value.Text);

   public override double Visit (NIdentifier identifier)
      => mDict[identifier.Name.Text];

   public override double Visit (NCast cast)
      => cast.Expr.Accept (this);

   public override double Visit (NUnary unary) {
      double d = unary.Expr.Accept (this);
      if (unary.Op.Kind == SUB) d = -d;
      return d;
   }

   public override double Visit (NFnCall fn) {
      var args = fn.Params.Select (x => x.Accept (this)).ToArray ();
      return fn.Name.Text.ToLower () switch {
         "sin" => Math.Sin (D2R (args[0])),
         "cos" => Math.Cos (D2R (args[0])),
         "tan" => Math.Tan (D2R (args[0])),
         "log" => Math.Log (args[0]),
         "exp" => Math.Exp (args[0]),
         "asin" => R2D (Math.Asin (args[0])),
         "acos" => R2D (Math.Acos (args[0])),
         "atan" => R2D (Math.Atan (args[0])),
         "atan2" => R2D (Math.Atan2 (args[0], args[1])),
         "random" => Random.Shared.Next (),
         "round" => args.Length == 1 ? Math.Round (args[0]) : Math.Round (args[0], (int)args[1]),
         _ => throw new NotImplementedException ()
      };

      static double R2D (double f) => f * 180 / Math.PI;
      static double D2R (double f) => f * Math.PI / 180;
   }

   public override double Visit (NBinary binary) {
      double a = binary.Left.Accept (this), b = binary.Right.Accept (this);
      return binary.Op.Kind switch {
         ADD => a + b, SUB => a - b, MUL => a * b, DIV => a / b,
         _ => throw new NotImplementedException ()
      };
   }
}