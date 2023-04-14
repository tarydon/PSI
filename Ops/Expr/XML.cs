namespace PSI;

public class ExprXML : Visitor<XElement> {
   public override XElement Visit (NLiteral literal) {
      XElement node = new ("Literal");
      node.SetAttributeValue ("Value", literal.Value.Text);
      node.SetAttributeValue ("Type", literal.Type);
      return node;
   }

   public override XElement Visit (NIdentifier identifier) {
      XElement node = new ("Ident");
      node.SetAttributeValue ("Name", identifier.Name);
      node.SetAttributeValue ("Type", identifier.Type);
      return node;
   }

   public override XElement Visit (NUnary unary) {
      XElement node = new ("Unary");
      node.SetAttributeValue ("Op", unary.Op.Kind);
      node.SetAttributeValue ("Type", unary.Type);
      node.Add (unary.Expr.Accept (this));
      return node;
   }

   public override XElement Visit (NBinary binary) {
      XElement node = new ("Binary");
      node.SetAttributeValue ("Op", binary.Op.Kind);
      node.SetAttributeValue ("Type", binary.Type);
      node.Add (binary.Left.Accept (this)); node.Add (binary.Right.Accept (this));
      return node;
   }
}