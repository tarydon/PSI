using PSI;
using PSI.Ops;
using System.Diagnostics;

static class Start {
   static void Main () {
      // Test5
      var expr = "round ((-sin (12 + pi + atan2 (12, 13.5)) + log(pi/3)), 4)";
      // "12.0 + pi + round (sin(3.5), 2) + atan2(12, 13.5) + length (\"Hello\") + random ()";
      Dictionary<string, double> vars = new () { ["pi"] = Math.PI, ["two"] = 2 };
      var node = new Parser (new Tokenizer (expr)).Parse ();

      // Evaluate
      var value = node.Accept (new ExprEvaluator (vars));
      Console.WriteLine ($"Value = {value}");

      // IL
      var types = vars.ToDictionary (x => x.Key, x => NType.Real);
      var type = node.Accept (new ExprTyper (types));
      var ilgen = new ExprILGen (vars);
      var il = node.Accept (ilgen);
      Console.WriteLine ($"\nIL = \n{il}");
      // Assemble the saved IL from commandline using command: 
      //    C:\Windows\Microsoft.NET\Framework\v4.0.30319\ilasm.exe\ilasm.exe App.il 
      // To assemble and run:
      //    C:\Windows\Microsoft.NET\Framework\v4.0.30319\ilasm.exe\ilasm.exe App.il && App.exe
      ilgen.Save ("App.il");
      Console.Write ("+ Assembling App.il to EXE...");
      var pi = new ProcessStartInfo ("C:/Windows/Microsoft.NET/Framework/v4.0.30319/ilasm.exe", "App.il") { UseShellExecute = true };
      var P = Process.Start (pi);
      if (P != null) {
         P.WaitForExit ();
         if (P.ExitCode == 0) {
            Console.WriteLine ("Done");
            Console.WriteLine ("+ Running App.exe...");
            P = Process.Start ("App.exe");
         } else {
            // Display compilation errors.
            pi.UseShellExecute = false;
            P = Process.Start (pi);
         }
         P?.WaitForExit ();
      }
      Console.Write ("\nPress any key..."); Console.ReadKey (true);


      Test1 ();      // Test ExprEval and ExprILGen
      Test2 ();      // Test ExprTyper and ExprGrapher
      Test3 ();      // Type checks on various expressions
      Test4 ();      // Tokenizer - printout of invalid token
   }

   // Test ExprEval and ExprILGen
   static void Test1 () {
      string expr = "(3 + 2) * 4 - 17 * -five * (two + 1 + 4 + 5)";
      var node = new Parser (new Tokenizer (expr)).Parse ();

      Console.WriteLine ("-----------------");
      Console.WriteLine ($"Expression = {expr}");
      Dictionary<string, double> vars = new () { ["five"] = 5, ["two"] = 2 };
      var value = node.Accept (new ExprEvaluator (vars));
      Console.WriteLine ($"Value = {value}");
      
      var il = node.Accept (new ExprILGen (vars));
      Console.WriteLine ($"\nIL Code = \n{il}");

      var types = vars.ToDictionary (x => x.Key, x => NType.Int);
      var type = node.Accept (new ExprTyper (types));
      var xml = node.Accept (new ExprXML ());
      Console.WriteLine ($"\nXML = \n{xml}");

      Console.Write ("\nPress any key..."); Console.ReadKey (true);
   }

   // Test type-assignment, graph generation
   static void Test2 () {
      string expr = "sin(pi + 3.5) + Round (2*3.55 + cos (0.5), 1) <= 1 <> \"Hello\" + two > true + \"World\"";
      var node = new Parser (new Tokenizer (expr)).Parse ();

      Console.WriteLine ("-----------------");
      Console.WriteLine ($"Expression = {expr}");
      Dictionary<string, NType> types = new () { ["pi"] = NType.Real, ["two"] = NType.Int };
      NType type = node.Accept (new ExprTyper (types));
      Console.WriteLine ($"Type = {type}");

      var graph = new ExprGrapher (expr);
      node.Accept (graph);
      Directory.CreateDirectory ("c:/etc");
      graph.SaveTo ("c:/etc/test.html");
      var pi = new ProcessStartInfo ("c:/etc/test.html") { UseShellExecute = true };
      Process.Start (pi);
      //Console.Write ("\nPress any key..."); Console.ReadKey (true);
   }

   // Type checks of various expressions
   static void Test3 () {
      Console.WriteLine ("-----------------");
      string[] mTypeCheck = new[] {
         "12", "13.5", "true", "\"hello!\"", "'A'", "-12", "-13.5", "five", "pi", "-two",
         "12 + 3", "13.5 - 3.5", "3 * 4", "13.3 / 3.5", "13 / 3.5", "3.5 * 2", "\"A\" + \"B\"",
         "'A' + 'B'", "\"A\" + true", "13.5 + \"B\"", "3 < 2", "3.5 < 2", "\"ABC\" < \"DEF\"",
         "'a' < 'b'", "true < false", "true = true", "3.5 = 2", "\"ABC\" = \"abc\"", "3 = true",
         "3 and 4", "true and false", "2 and not 7", "12 mod 3"
      };
      Dictionary<string, NType> types = new () { ["five"] = NType.Int, ["pi"] = NType.Real, ["two"] = NType.Int };
      foreach (var s in mTypeCheck) {
         var node = new Parser (new Tokenizer (s)).Parse ();
         var type = node.Accept (new ExprTyper (types));
         Console.WriteLine ($"{s,20} : {type}");
      }
      Console.Write ("\nPress any key..."); Console.ReadKey (true);
   }

   // Tokenizer test of valid and invalid programs
   static void Test4 () {
      Console.WriteLine ("-----------------");
      Console.WriteLine ("Valid program");
      var tokenizer = new Tokenizer (Prog0);
      int line = 1;
      for (; ; ) {
         var token = tokenizer.Next ();
         if (token.Kind == Token.E.EOF) break;
         while (line < token.Line) { Console.WriteLine (); line++; }
         Console.Write ($"{token}  ");
      }
      Console.WriteLine (); Console.WriteLine ();

      Console.WriteLine ("Testing invalid program:");
      var prog1 = Prog0.Replace ("prod * i;", "prod * i?");
      tokenizer = new Tokenizer (prog1);
      for (; ; ) {
         var token = tokenizer.Next ();
         if (token.Kind == Token.E.ERROR) { token.PrintError (); break; }
         if (token.Kind == Token.E.EOF) break;
      }
      Console.WriteLine ();
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