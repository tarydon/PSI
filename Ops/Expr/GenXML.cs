namespace PSI;
using System.Xml.Linq;

// An basic IL code generator, implemented using the Visitor pattern
public class ExprXML : Visitor<XElement> {
   public override XElement Visit (NLiteral literal)
      => new ("Literal", new XAttribute ("Value", literal.Value),
                         new XAttribute ("Type", literal.Type));

   public override XElement Visit (NIdentifier identifier)
      => new ("Ident", new XAttribute ("Name", identifier.Name),
                       new XAttribute ("Type", identifier.Type));

   public override XElement Visit (NUnary unary)
      => new ("Unary", new XAttribute ("Op", unary.Op.Kind),
                       new XAttribute ("Type", unary.Type),
                       unary.Expr.Accept (this));

   public override XElement Visit (NBinary binary)
      => new ("Binary", new XAttribute ("Op", binary.Op.Kind),
                        new XAttribute ("Type", binary.Type),
                        binary.Left.Accept (this),
                        binary.Right.Accept (this));
}
