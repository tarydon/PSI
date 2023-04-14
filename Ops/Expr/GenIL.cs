namespace PSI;
using static Token.E;

// An basic IL code generator, implemented using the Visitor pattern
public class ExprILGen : Visitor<StringBuilder> {
   StringBuilder mSB = new ();
   int mID = 0;

   public override StringBuilder Visit (NLiteral literal)
      => mSB.AppendLine ($"{Label} ldc.i4 {literal.Value.Text}");

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

   public override StringBuilder Visit (NFnCall nFnCall) {
      foreach (var p in nFnCall.Params) p.Accept (this);
      return mSB.AppendLine ($"{Label} call {nFnCall.Name.Text}");
   }

   string Label => $"IL{++mID:D3}:";
}