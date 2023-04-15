namespace PSI;
using static Token.E;
using static NType;

// An basic IL code generator, implemented using the Visitor pattern
public class ExprILGen : Visitor<StringBuilder> {
   StringBuilder mSB = new ();
   int mID = 0;

   public override StringBuilder Visit (NLiteral literal) {
      string ilcode = literal.Type switch { 
         Real => "ldc.r8", String => "ldstr", _ => "ldc.i4"
      };
      string value = literal.Value.Text;
      if (literal.Type == String) value = value.Quoted ();
      return mSB.AppendLine ($"{Label} {ilcode} {value}");
   }

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

   public override StringBuilder Visit (NFnCall fn) {
      fn.Params.ForEach (a => a.Accept (this));
      return mSB.AppendLine ($"{Label} call {fn.Name.Text}");
   }

   string Label => $"IL{++mID:D3}:";
}