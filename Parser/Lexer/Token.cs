// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Token.cs ~ Represents the lexical analysis Tokens for PSI
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;
using static Token.E;

// Represents a PSI language Token
public class Token {
   public Token (Tokenizer? source, E kind, string text, int line, int column) 
      => (Source, Kind, Text, Line, Column) = (source, kind, text, line, column);
   public Tokenizer? Source { get; }
   public E Kind { get; }
   public string Text { get; }
   public int Line { get; }
   public int Column { get; }

   // The various types of token
   public enum E {
      // Keywords
      PROGRAM, VAR, IF, THEN, WHILE, ELSE, FOR, TO, DOWNTO,
      DO, BEGIN, END, PRINT, TYPE, NOT, OR, AND, MOD, WRITE,
      WRITELN, READ, LABEL, INTEGER, REAL, BOOLEAN, STRING, CHAR,
      PROCEDURE, FUNCTION, REPEAT, UNTIL, _ENDKEYWORDS,
      // Operators
      ADD, SUB, MUL, DIV, NEQ, LEQ, GEQ, EQ, LT, GT, ASSIGN, 
      _ENDOPERATORS,
      // Punctuation
      SEMI, PERIOD, COMMA, OPEN, CLOSE, COLON, 
      _ENDPUNCTUATION,
      // Others
      IDENT, L_INTEGER, L_REAL, L_BOOLEAN, L_STRING, L_CHAR, EOF, ERROR
   }

   // Print a Token
   public override string ToString () => Kind switch {
      EOF or ERROR => Kind.ToString (),
      < _ENDKEYWORDS => $"\u00ab{Kind.ToString ().ToLower ()}\u00bb",
      L_STRING => $"\"{Text}\"",
      L_CHAR => $"'{Text}'",
      _ => Text
   };

   // Utility function used to echo an error to the console
   public void PrintError () {
      if (Kind != ERROR) throw new Exception ("PrintError called on a non-error token");
      if (Source != null) {
         const int gutter = 5; 
         var (lines, title) = (Source.Lines, $"File: {Source.FileName}");
         Console.WriteLine (title);
         Console.WriteLine ("┬".PadLeft (gutter, '─').PadRight (title.Length, '─'));
         for (int i = Line - 2; i <= Line + 2; i++) {
            if (i < 1 || i > lines.Length) continue;
            Console.WriteLine ($"{i, gutter - 1}|{lines[i - 1]}");
            if (i == Line) {
               Console.ForegroundColor = ConsoleColor.Yellow;
               Console.WriteLine ("^".PadLeft (Column + gutter)); // Error pointer
               int totalWidth = Column + gutter + Text.Length / 2;
               Console.WriteLine (Text.PadLeft (totalWidth));
               Console.ResetColor ();
            }
         }
      } else {
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.WriteLine ($"At line {Line}, column {Column}: {Text}");
         Console.ResetColor ();
      }
   }

   // Helper used by the parser (maps operator sequences to E values)
   public static readonly List<(E Kind, string Text)> Match = new () {
      (NEQ, "<>"), (LEQ, "<="), (GEQ, ">="), (ASSIGN, ":="), (ADD, "+"),
      (SUB, "-"), (MUL, "*"), (DIV, "/"), (EQ, "="), (LT, "<"),
      (LEQ, "<="), (GT, ">"), (SEMI, ";"), (PERIOD, "."), (COMMA, ","),
      (OPEN, "("), (CLOSE, ")"), (COLON, ":")
   };
}
