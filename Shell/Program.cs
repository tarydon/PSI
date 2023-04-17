using PSI;
using static PSI.NType;

static class Start {
   static void Main () {
      string expr = "pi + (five * -3) + sin(3.5) + atan2(5, 2) + length(\"Hello\") + random()";

      Dictionary<string, NType> types = new () {
         ["pi"] = Real, ["five"] = Int, ["sin"] = Real, ["atan2"] = Real, 
         ["random"] = Real, ["length"] = Int
      };
      var node = new Parser (new Tokenizer (expr)).Parse ();
      node.Accept (new ExprTyper (types));
      var xml = node.Accept (new ExprXML ());
      xml.Save ("c:/etc/test.xml");
      Console.WriteLine (xml);
   }
}