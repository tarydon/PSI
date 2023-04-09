namespace PSI.Ops;
using static Token.E;

// An expression evaluator, implementing using the visitor pattern
public class ExprEvaluator : Visitor<int> {
   public ExprEvaluator (Dictionary<string, int> dict) => mDict = dict;
   Dictionary<string, int> mDict;

   public override int Visit (NLiteral literal)
      => int.Parse (literal.Value.Text);

   public override int Visit (NIdentifier identifier)
      => mDict[identifier.Name.Text];

   public override int Visit (NUnary unary) {
      int d = unary.Expr.Accept (this);
      if (unary.Op.Kind == SUB) d = -d;
      return d;
   }

   public override int Visit (NBinary binary) {
      int a = binary.Left.Accept (this), b = binary.Right.Accept (this);
      return binary.Op.Kind switch {
         ADD => a + b, SUB => a - b, MUL => a * b, DIV => a / b,
         _ => throw new NotImplementedException ()
      };
   }
}