namespace PSI;
using static Token.E;

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
      Console.OutputEncoding = Encoding.Unicode;
      if (Kind != ERROR) throw new Exception ("PrintError called on a non-error token");
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write ($"File: {Source.FileName}");
      var cPos = Console.CursorLeft;
      Console.WriteLine ($"\n\u2500\u2500\u2500\u252c{string.Join ("", Enumerable.Repeat ("\u2500", cPos - 3))}");
      var lines = Source.Lines;
      if (Line < lines.Length - 3) Console.WriteLine ($"{Line - 2,3}\u2502{lines[Line - 3]}");
      if (Line < lines.Length - 2) Console.WriteLine ($"{Line - 1,3}\u2502{lines[Line - 2]}");
      if (Line < lines.Length - 1) Console.WriteLine ($"{Line,3}\u2502{lines[Line - 1]}");
      cPos = Column + lines[Line - 1].TakeWhile (a => a is ' ').Count () - 1;
      Console.CursorLeft = cPos;
      Console.WriteLine ("^");
      int endPos = cPos + Text.Length / 2;
      Console.CursorLeft = 1 + cPos - Text.Length / 2 - (endPos >= Console.WindowWidth ? Console.WindowWidth - endPos : 0);
      Console.WriteLine ($"{Text}");
      if (Line < lines.Length + 2) Console.WriteLine ($"{Line + 1,3}\u2502{lines[Line + 1]}");
      if (Line < lines.Length + 1) Console.WriteLine ($"{Line + 2,3}\u2502{lines[Line + 2]}");
      Console.ResetColor ();
   }

   // Helper used by the parser (maps operator sequences to E values)
   public static List<(E Kind, string Text)> Match = new () {
      (NEQ, "<>"), (LEQ, "<="), (GEQ, ">="), (ASSIGN, ":="), (ADD, "+"),
      (SUB, "-"), (MUL, "*"), (DIV, "/"), (EQ, "="), (LT, "<"),
      (LEQ, "<="), (GT, ">"), (SEMI, ";"), (PERIOD, "."), (COMMA, ","),
      (OPEN, "("), (CLOSE, ")"), (COLON, ":")
   };
}
