using PSI;

static class Start {
   static void Main () {
      NProgram? node;
      foreach (var file in Directory.GetFiles ("../Shell/Demo", "*.pas")) {
         //var file = "../Shell/Demo/Basic.pas";
         Console.WriteLine ($"\n\n[{Path.GetFileName (file)}]\n");
         try {
            var text = File.ReadAllText (file);
            node = new Parser (new Tokenizer (text)).Parse ();
            node.Accept (new TypeAnalyze ());
            node.Accept (new PSIPrint ());
         } catch (ParseException pe) {
            pe.Print ();
         } catch (Exception e) {
            Console.WriteLine ();
            Console.WriteLine (e);
         }
      }
   }
}