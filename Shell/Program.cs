using System.Diagnostics;
using PSI;

static class Start {
   static void Main () {
      string program = "P:/TData/Compile/Comp0.pas";

      var ps = new PSICompiler ();
      if (ps.Compile (program)) {
         var line = new string ('\u2500', Console.WindowWidth - 1);
         Console.WriteLine (line);
         Console.WriteLine (File.ReadAllText (program).Trim ());
         Console.WriteLine (line);

         var pi = new ProcessStartInfo ("P:/Output/PSIOutput.exe", "");
         Console.ForegroundColor = ConsoleColor.DarkBlue;
         var process = Process.Start (pi)!; process.WaitForExit ();
         Console.ResetColor ();
         Console.WriteLine (line);
         Console.WriteLine ("Process returned code: {0}", process.ExitCode);
      }
   }
} 