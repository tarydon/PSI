// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// ExprXML.cs ~ Dumps a syntax tree to XML
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;

public class ExprXML : Visitor<XElement> {
   public override XElement Visit (NLiteral t)
      => new ("Literal", new XAttribute ("Value", $"{t.Value.Text} : {t.Type}"));

   public override XElement Visit (NIdentifier d)
      => new ("Ident", new XAttribute ("Name", $"{d.Name} : {d.Type}"));

   public override XElement Visit (NUnary u)
      => new ("Unary", new XAttribute ("Op", $"{u.Op.Text} : {u.Type}"), 
         u.Expr.Accept (this));

   public override XElement Visit (NBinary b)
      => new ("Binary", new XAttribute ("Op", $"{b.Op.Text} : {b.Type}"),
         b.Left.Accept (this), b.Right.Accept (this));

   public override XElement Visit (NFnCall f) {
      var node = new XElement ("FnCall", new XAttribute ("Name", $"{f.Name} : {f.Type}"));
      f.Params.ForEach (a => node.Add (a.Accept (this)));
      return node;
   }
}