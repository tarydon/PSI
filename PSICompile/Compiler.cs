// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Compiler.cs : Impements a Pascal -> Exe compiler            
// ─────────────────────────────────────────────────────────────────────────────
using System.Diagnostics;
namespace PSI;

public class PSICompiler {
   // Compiles the given Pascal source file to P:/Output/PSIOutput.dll
   public bool Compile (string source) {
      try {
         Console.WriteLine ($"Compiling {source}...");
         var text = File.ReadAllText (source);
         var node = new Parser (new Tokenizer (text)).Parse ();
         var codegen = new ILCodeGen ();
         node.Accept (codegen);

         File.WriteAllText ("P:/Output/PSIOutput.il", codegen.S.ToString ());
         var pi = new ProcessStartInfo ("P:/Bin/AsmCore/ILAsm.exe",
            "/QUIET /dll /PE64 /X64 P:/Output/PSIOutput.il /output=P:/Output/PSIOutput.dll");
         var process = Process.Start (pi)!;
         process.WaitForExit ();
         if (process.ExitCode != 0) throw new Exception ($"ILASM returned error code {process.ExitCode}");
         Console.WriteLine ("Done.");
         return true;

      } catch (ParseException pe) {
         Console.WriteLine ();
         pe.Print ();
      } catch (Exception e) {
         Console.WriteLine ();
         Console.WriteLine (e);
      }
      return false;
   }
}