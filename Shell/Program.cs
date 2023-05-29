using System.Diagnostics;
using PSI;

static class Start {
   static void Main () {
      string program = "P:/TData/Compile/Comp7.pas";

      var ps = new PSICompiler ();
      if (ps.Compile (program)) {
         Console.WriteLine ("---------------------------------");
         Console.WriteLine (File.ReadAllText (program).Trim ());
         Console.WriteLine ("---------------------------------");
         var pi = new ProcessStartInfo ("P:/Output/PSIOutput.exe", "");
         var process = Process.Start (pi)!; process.WaitForExit ();
         Console.WriteLine ("---------------------------------");
         Console.WriteLine ("Process returned code: {0}", process.ExitCode);
      }
   }
} 