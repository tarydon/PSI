namespace PSI;

// ExprXML renders an expression tree to XML - it's implemented using
// the Visitor pattern
public class ExprXML : Visitor<XElement> {
   public override XElement Visit (NLiteral literal)
      => new ("Literal", XAttr ("Value", literal.Value.Text), XType (literal));

   public override XElement Visit (NIdentifier identifier)
      => new ("Identifier", XAttr ("Name", identifier.Name.Text), XType (identifier));

   public override XElement Visit (NUnary unary)
      => new ("Unary", XAttr ("Op", unary.Op.Kind), XType (unary), 
         Child (unary.Expr));

   public override XElement Visit (NBinary binary)
      => new ("Binary", XAttr ("Op", binary.Op.Kind), XType (binary), 
         Child (binary.Left), Child (binary.Right));

   // Helpers ....................................
   static XAttribute XAttr (string name, object value) => new XAttribute (name, value);
   static XAttribute XType (NExpr expr) => XAttr ("Type", expr.Type);
   XElement Child (NExpr expr) => expr.Accept (this);
}