using System.Diagnostics;

namespace PSITest;

public class TestFailException : Exception {
   public TestFailException (string message) : base (message) { }
}

public static class Assert {
   public static void TextFilesEqual (string reference, string test) {
      if (!File.Exists (reference)) {
         File.Copy (test, reference);
         return;
      }
      string s1 = File.ReadAllText (reference).Replace ("\r\n", "\n");
      string s2 = File.ReadAllText (test).Replace ("\r\n", "\n");
      if (s1 == s2) return;

      var process = Process.Start ("winmergeu.exe", $"\"{reference}\" \"{test}\"");
      process.WaitForExit ();
      throw new TestFailException ($"Files different: {reference} and {test}");
   }
}