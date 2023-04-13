namespace PSI;
using System.Xml.Linq;

// An basic IL code generator, implemented using the Visitor pattern
public class ExprXML : Visitor<XElement> {

   public override XElement Visit (NLiteral literal)
      => new ("Literal", new XAttribute ("Value", literal.Value), new XAttribute ("Type", literal.Type));

   public override XElement Visit (NIdentifier identifier)
      => new ("Ident", new XAttribute ("Name", identifier.Name), new XAttribute ("Type", identifier.Type));

   public override XElement Visit (NUnary unary) {
      var child = unary.Expr.Accept (this);
      XElement uXML = new ("Unary", new XAttribute ("Op", unary.Op), new XAttribute ("Type", unary.Type));
      uXML.Add (child);
      return uXML;
   }

   public override XElement Visit (NBinary binary) {
      var left = binary.Left.Accept (this); var right = binary.Right.Accept (this);
      XElement bXML = new ("Binary", new XAttribute ("Op", binary.Op), new XAttribute ("Type", binary.Type));
      bXML.Add (left); bXML.Add (right);
      return bXML;
   }

   public override XElement Visit (NFnCall function) {
      XElement fXML = new ("Function", new XAttribute ("Name", function.Name), new XAttribute ("Type", function.Type));
      foreach (var a in function.Params) fXML.Add (a.Accept (this));
      return fXML;
   }
}