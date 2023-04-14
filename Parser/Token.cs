namespace PSI;
using static Token.E;
using static Console;

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
      string s = $"File: {Source.FileName}", msg = $"{s}\r\n{new string ('\u2500', s.Length)}\r\n";
      var (idx, n, space) = (Line - 2, Source.Lines.Length, new string (" "));
      for (int i = idx; i <= Line; i++, idx++)
         if (idx >= 1) msg += $"{idx,4}\u2502{space}{Source.Lines[idx - 1]}\r\n";
      Write (msg);
      ForegroundColor = ConsoleColor.Yellow; var nSpaces = Column + 5;
      CursorLeft = nSpaces;
      Write ("^\r\n");
      CursorLeft = Math.Max (0, (nSpaces - Text.Length / 2) + 1);
      WriteLine (Text);
      ResetColor (); msg = "";
      for (int i = 0; i < 2 && idx <= n; i++, idx++) 
         msg += $"{idx,4}\u2502{space}{Source.Lines[idx - 1]}\r\n";
      Write (msg);
   }

   // Helper used by the parser (maps operator sequences to E values)
   public static List<(E Kind, string Text)> Match = new () {
      (NEQ, "<>"), (LEQ, "<="), (GEQ, ">="), (ASSIGN, ":="), (ADD, "+"),
      (SUB, "-"), (MUL, "*"), (DIV, "/"), (EQ, "="), (LT, "<"),
      (LEQ, "<="), (GT, ">"), (SEMI, ";"), (PERIOD, "."), (COMMA, ","),
      (OPEN, "("), (CLOSE, ")"), (COLON, ":")
   };
}
