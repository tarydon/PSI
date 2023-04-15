using PSI;
using PSI.Ops;
using System.Diagnostics;

static class Start {
   static void Main () { 
      Test ();
   }

   // Test ExprTyper, ExprILGen, ExprGrapher
   static void Test () {
      Console.WriteLine ("IL Code: ");
      string expr = "pi + sin(3.5) + atan2(five, 2.5) + length(\"Hello\") + random()";
      var node = new Parser (new Tokenizer (expr)).Parse ();

      // Assign types to each of the nodes
      Dictionary<string, NType> types = new () {
         ["pi"] = NType.Real, ["sin"] = NType.Real, ["atan2"] = NType.Real,
         ["five"] = NType.Int, ["length"] = NType.Int, ["random"] = NType.Real
      };
      node.Accept (new ExprTyper (types));

      // Generate IL code for the expression
      var il = node.Accept (new ExprILGen ());
      Console.WriteLine ($"\nIL Code = \n{il}");

      // Generate a graph for the expression
      var graph = new ExprGrapher (expr);
      node.Accept (graph);
      Directory.CreateDirectory ("c:/etc");
      graph.SaveTo ("c:/etc/test.html");
      var pi = new ProcessStartInfo ("c:/etc/test.html") { UseShellExecute = true };
      Process.Start (pi);

      Console.WriteLine ("----------------");
      Console.WriteLine (node.Accept (new ExprXML ()));
      Console.Write ("\nPress any key..."); Console.ReadKey (true);
   }

   static string Prog0 = """
      program Expr;
      var
        i, fib: integer;

      function Fibo (n: integer) : integer;
      var 
        i, prod: integer;
      begin 
        prod := 1;
        for i := 1 to n do begin
          prod := prod * i;
        end
        Fibo := prod;
      end;

      begin
        for i := 1 to 10 do begin
          fib := Fibo (i);
          WriteLn ("Fibo(", i, ") = ", fib);
        end
      end.
      """;
}