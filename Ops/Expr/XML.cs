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

   public override XElement Visit (NFnCall fn) {
      var elem = new XElement ("FnCall", KV ("Name", fn.Name.Text), Type (fn));
      if (fn.Params.Length > 0) {
         var args = new XElement ("Params"); elem.Add (args);
         foreach (var arg in fn.Params)
            args.Add (arg.Accept (this));
      }
      return elem;
   }

   XAttribute Op (Token op) => new ("Op", op.Kind);
   XAttribute Type (NExpr exp) => new ("Type", exp.Type);
   XAttribute KV (string key, object value) => new (key, value);
}