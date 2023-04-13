namespace PSI;
using static Token.E;

// An basic IL code generator, implemented using the Visitor pattern
public class ExprILGen : Visitor<StringBuilder> {
   StringBuilder mSB = new ();
   int mID = 0;

   public override StringBuilder Visit (NLiteral literal) {
      string load = literal.Type switch {
         NType.Int or NType.Boolean => "ldc.i4",
         NType.Real => "ldc.r4",
         NType.String => "ldstr",
         _ => throw new NotImplementedException ()
      };
      string value = literal.Type is NType.Boolean ? (literal.Value.Text is "false" ? "0" : "1") : literal.Value.Text;
      return mSB.AppendLine ($"{Label} {load} {value}"); }

   public override StringBuilder Visit (NIdentifier identifier)
      => mSB.AppendLine ($"{Label} ldlocal {identifier.Name.Text}");

   public override StringBuilder Visit (NUnary unary) {
      unary.Expr.Accept (this);
      if (unary.Op.Kind == SUB) mSB.AppendLine ($"{Label} neg");
      return mSB;
   }

   public override StringBuilder Visit (NBinary binary) {
      binary.Left.Accept (this); binary.Right.Accept (this);
      return mSB.AppendLine ($"{Label} {binary.Op.Kind.ToString ().ToLower ()}");
   }

   public override StringBuilder Visit (NFnCall function) {
      foreach (var a in function.Params) a.Accept (this);
      return mSB.AppendLine ($"{Label} call {function.Name.ToString ().ToLower ()}");
   }

   string Label => $"IL{++mID:D3}:";
}