using PSI;
using PSI.Ops;
using System.Diagnostics;

static class Start {
   static void Main () {
      Dictionary<string, NType> types = new () { 
         ["five"] = NType.Int, ["two"] = NType.Int, ["pi"] = NType.Real, 
         ["message"] = NType.String 
      };

      foreach (var s in mTypeCheck) {
         var node = new Parser (new Tokenizer (s)).Parse ();
         var type = node.Accept (new ExprTyper (types));
         Console.WriteLine ($"{s,20} : {type}");
      }

      var node1 = new Parser (new Tokenizer (expr0)).Parse ();
      node1.Accept (new ExprTyper (types));
      var graph = new ExprGrapher (expr0);
      node1.Accept (graph);
      graph.SaveTo ("c:/etc/test.html");
      var pi = new ProcessStartInfo ("c:/etc/test.html") { UseShellExecute = true };
      Process.Start (pi);
   }
   // static string expr0 = "(3 + 2) * 4 - 17 * -five * (two + 1 + 4 + 5)";
   static string expr0 = "(pi + 3.5) + 2 <= 1 <> \"Hello\" + two > true + \"World\"";
   // static string expr0 = "\"Hello\" + ((3 < 1) and (4 mod 2 > 3) or not (2 > 1 and not 3))";
   // static string expr0 = "\"Hello\" + not (3 > 2) and ('A' < 'B')";

   static string[] mTypeCheck = new[] {
      "12", "13.5", "true", "\"hello!\"", "'A'", "-12", "-13.5", "five", "pi", "-two",
      "12 + 3", "13.5 - 3.5", "3 * 4", "13.3 / 3.5", "13 / 3.5", "3.5 * 2", "\"A\" + \"B\"", 
      "'A' + 'B'", "\"A\" + true", "13.5 + \"B\"", "3 < 2", "3.5 < 2", "\"ABC\" < \"DEF\"", 
      "'a' < 'b'", "true < false", "true = true", "3.5 = 2", "\"ABC\" = \"abc\"", "3 = true",
      "3 and 4", "true and false", "2 and not 7", "12 mod 3"
   };
}