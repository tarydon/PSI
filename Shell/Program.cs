using System.Diagnostics;
using PSI;

static class Start {
   static void Main () {
      var ps = new PSICompiler ();
      ps.Compile ("P:/TData/Compile/Hello.pas");

      var pi = new ProcessStartInfo ("P:/Output/PSIOutput.exe", "");
      var process = Process.Start (pi)!; process.WaitForExit ();
      Console.WriteLine ("Process returned code: {0}", process.ExitCode);
   }
} 