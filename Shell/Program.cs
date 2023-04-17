using PSI;
using static PSI.NType;

static class Start {
   static void Main () {
      NProgram? node;
      try {
         var text = File.ReadAllText ("P:/Demo/Hello.pas");
         node = new Parser (new Tokenizer (text)).Parse ();
         node.Accept (new PSIPrint ());
      } catch (ParseException pe) {
         pe.Print ();
      } catch (Exception e) {
         Console.WriteLine (e);
      }
   }
}