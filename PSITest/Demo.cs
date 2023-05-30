using PSI;
namespace PSITest;

[TestFixture (1, "Basic DEMO files that all compile correctly")]
class DemoFiles {
   [Test (1, "Test of Demo/BASIC.pas")]
   void Test1 () => Test ("Demo/Basic.pas");

   [Test (2, "Test of Demo/COMPLEX.pas")]
   void Test2 () => Test ("Demo/Complex.pas");

   [Test (3, "Test of Demo/FIBO.pas")]
   void Test3 () => Test ("Demo/Fibo.pas");

   [Test (4, "Test of Demo/MULTIPARAMS.pas")]
   void Test4 () => Test ("Demo/MultiParams.pas");

   [Test (5, "Test of Demo/NESTED.pas")]
   void Test5 () => Test ("Demo/Nested.pas");

   [Test (6, "Test of Demo/CONST.pas")]
   void Test6 () => Test ("Demo/Const.pas");

   static public void Test (string file) {
      file = $"{Program.Root}/{file}";
      string outfile = Path.ChangeExtension (file, ".txt");
      string tmpfile = $"{Program.Root}/tmpfile.txt";
      NProgram? node;
      string output;

      try {
         var text = File.ReadAllText (file);
         node = new Parser (new Tokenizer (text)).Parse ();
         output = node.Accept (new PSIPrint (silent: true)).ToString ();
      } catch (ParseException pe) {
         output = pe.Context ();
      }

      File.WriteAllText (tmpfile, output);
      Assert.TextFilesEqual (outfile, tmpfile);
   }
}