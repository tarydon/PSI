namespace PSI.Ops;
using System.Xml.Linq;

public class ExprXML : Visitor<XElement> {
   public override XElement Visit (NLiteral lit) => new ("Literal", new XAttribute ("Value", lit.Value), new XAttribute ("Type", lit.Type));

   public override XElement Visit (NIdentifier ident) => new ("Ident", new XAttribute ("Name", ident.Name), new XAttribute ("Type", ident.Type));

   public override XElement Visit (NUnary unary) {
      var child = unary.Expr.Accept (this);
      XElement elem = new ("Unary", new XAttribute ("Op", unary.Op.Kind), new XAttribute ("Type", unary.Type));
      elem.Add (child);
      return elem;
   }

   public override XElement Visit (NBinary binary) {
      var (e1, e2) = (binary.Left.Accept (this), binary.Right.Accept (this));
      XElement elem = new ("Binary", new XAttribute ("Op", binary.Op.Kind), new XAttribute ("Type", binary.Type));
      elem.Add (e1); elem.Add (e2);
      return elem;
   }
}
