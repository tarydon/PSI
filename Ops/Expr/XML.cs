namespace PSI;
using System.Xml.Linq;

public class ExprXML : Visitor<XElement> {
   public override XElement Visit (NLiteral literal) 
      => new ("Literal", KV ("Value", literal.Value), Type (literal));

   public override XElement Visit (NIdentifier identifier) 
      => new ("Ident", KV ("Name", identifier.Name.Text), Type (identifier));

   public override XElement Visit (NUnary unary) 
      => new ("Unary", Op (unary.Op), Type (unary), unary.Expr.Accept (this));

   public override XElement Visit (NBinary binary) 
      => new ("Binary", Op (binary.Op), Type (binary), 
         binary.Left.Accept (this), 
         binary.Right.Accept (this));

   XAttribute Op (Token op) => new ("Op", op.Kind);
   XAttribute Type (NExpr exp) => new ("Type", exp.Type);
   XAttribute KV (string key, object value) => new (key, value);
}