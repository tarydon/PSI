// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Compiler.cs : Impements a Pascal -> Exe compiler            
// ─────────────────────────────────────────────────────────────────────────────
using System.Diagnostics;
namespace PSI;

public class PSICompiler {
   // Compiles the given Pascal source file to P:/Output/PSIOutput.dll
   public bool Compile (string source) {
      try {
         // Read the source, and pass it to an ILCodeGen
         Console.WriteLine ($"Compiling {source}...");
         var text = File.ReadAllText (source);
         var node = new Parser (new Tokenizer (text)).Parse ();
         var codegen = new ILCodeGen ();
         node.Accept (codegen);

         // At this point, the ILCodeGen has generated IL code, save it to 
         // a file and call ILAsm to assemble it:
         File.WriteAllText ("P:/Output/PSIOutput.il", codegen.S.ToString ());
         var pi = new ProcessStartInfo (
            "P:/Bin/AsmCore/ILAsm.exe",
            "/QUIET /dll /PE64 /X64 P:/Output/PSIOutput.il /output=P:/Output/PSIOutput.dll");
         
         // If ILASM reports any errors, throw an exception 
         var process = Process.Start (pi)!;
         process.WaitForExit ();
         if (process.ExitCode != 0) throw new Exception ($"ILASM returned error code {process.ExitCode}");
         Console.WriteLine ("Done.");
         return true;

      } catch (ParseException pe) {
         // If there are any syntax errors in our Pascal code, we will get
         // a ParseException from the Parser - report that
         Console.WriteLine ();
         pe.Print ();
      } catch (Exception e) {
         // If there are any other exceptions, they usually indicate a bug
         // in our code
         Console.WriteLine ();
         Console.WriteLine (e);
      }
      return false;
   }
}