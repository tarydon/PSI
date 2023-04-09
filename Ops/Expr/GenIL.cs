namespace PSI;
using static Token.E;

// An basic IL code generator, implemented using the Visitor pattern
public class ExprILGen : Visitor<StringBuilder> {
   StringBuilder mSB = new ();
   int mID = 0;

   public override StringBuilder Visit (NLiteral literal)
      => mSB.AppendLine ($"IL{++mID:D3}: ldc.i4 {literal.Value.Text}");

   public override StringBuilder Visit (NIdentifier identifier)
      => mSB.AppendLine ($"IL{++mID:D3}: ldlocal {identifier.Name.Text}");

   public override StringBuilder Visit (NUnary unary) {
      unary.Expr.Accept (this);
      if (unary.Op.Kind == SUB) mSB.AppendLine ($"IL{++mID:D3}: neg");
      return mSB;
   }

   public override StringBuilder Visit (NBinary binary) {
      binary.Left.Accept (this); binary.Right.Accept (this);
      return mSB.AppendLine ($"IL{++mID:D3}: {binary.Op.Kind.ToString ().ToLower ()}");
   }
}