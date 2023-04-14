namespace PSI;
using static Token.E;
using static System.Console;

// Represents a PSI language Token
public class Token {
   public Token (Tokenizer source, E kind, string text, int line, int column) 
      => (Source, Kind, Text, Line, Column) = (source, kind, text, line, column);
   public Tokenizer Source { get; }
   public E Kind { get; }
   public string Text { get; }
   public int Line { get; }
   public int Column { get; }

   // The various types of token
   public enum E {
      // Keywords
      PROGRAM, VAR, IF, THEN, WHILE, ELSE, FOR, TO, DOWNTO,
      DO, BEGIN, END, PRINT, TYPE, NOT, OR, AND, MOD, _ENDKEYWORDS,
      // Operators
      ADD, SUB, MUL, DIV, NEQ, LEQ, GEQ, EQ, LT, GT, ASSIGN, 
      _ENDOPERATORS,
      // Punctuation
      SEMI, PERIOD, COMMA, OPEN, CLOSE, COLON, 
      _ENDPUNCTUATION,
      // Others
      IDENT, INTEGER, REAL, BOOLEAN, STRING, CHAR, EOF, ERROR
   }

   // Print a Token
   public override string ToString () => Kind switch {
      EOF or ERROR => Kind.ToString (),
      < _ENDKEYWORDS => $"\u00ab{Kind.ToString ().ToLower ()}\u00bb",
      STRING => $"\"{Text}\"",
      CHAR => $"'{Text}'",
      _ => Text,
   };

   // Utility function used to echo an error to the console
   public void PrintError () {
      if (Kind != ERROR) throw new Exception ("PrintError called on a non-error token");
      OutputEncoding = new UnicodeEncoding ();
      var fName = $"File: {Source.FileName}";
      WriteLine (fName);
      Console.Write (new string ('\u2500', 3) + '\u252C');
      WriteLine (new string ('\u2500', fName.Length - 3));
      var factor = 2;
      for (int i = 0; i < 3; i++) {
         var idx = Line - factor;
         if (idx > 0) Write (idx);
         factor--;
      }
      string currLine = $"{Line}\u2502{Source.Lines[Line - 1]}";
      ForegroundColor = ConsoleColor.Yellow;
      WriteLine (new string (' ', currLine.Length - 2) + "^");
      WriteLine (new string (' ', currLine.Length - 8) + Text);
      ResetColor ();
      factor = 1;
      for (int i = 0; i < 2; i++) {
         var idx = Line + factor;
         if (idx <= Source.Lines.Length) Write (idx);
         factor++;
      }

      void Write (int idx) {
         CursorLeft = idx.ToString ().Length == 1 ? 2 : 1;
         WriteLine ($"{idx}\u2502{Source.Lines[idx - 1]}");
      }
   }

   // Helper used by the parser (maps operator sequences to E values)
   public static List<(E Kind, string Text)> Match = new () {
      (NEQ, "<>"), (LEQ, "<="), (GEQ, ">="), (ASSIGN, ":="), (ADD, "+"),
      (SUB, "-"), (MUL, "*"), (DIV, "/"), (EQ, "="), (LT, "<"),
      (LEQ, "<="), (GT, ">"), (SEMI, ";"), (PERIOD, "."), (COMMA, ","),
      (OPEN, "("), (CLOSE, ")"), (COLON, ":")
   };
}
