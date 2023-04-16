﻿namespace PSI;
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
      if (Kind != ERROR) throw new Exception ("PrintError called on a non-error token");
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine ($"At line {Line}, column {Column} of {Source.FileName}: {Text}");
      Console.ResetColor ();
      // Print the title.
      var title = $"File: {Source.FileName}";
      Console.WriteLine (title);
      // Underline the title.
      const int gutter = 5;
      Console.WriteLine ("┬".PadLeft (gutter, '─').PadRight (title.Length, '─'));
      int start = Math.Max (0, Line - 3), end = Math.Min (Line + 1, Source.Lines.Length - 1);
      for (int i = start; i <= end; i++) {
         // Print the line number in the gutter area.
         Console.Write ($"{(i + 1),gutter - 1}│");
         Console.WriteLine (Source.Lines[i]);
         if (i == Line - 1) {
            // Error pointer.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine ("^".PadLeft (Column + gutter));
            // Print center-aligned error text.
            int totalWidth = (Column + gutter) + (Text.Length / 2);
            Console.WriteLine (Text.PadLeft (totalWidth));
            Console.ResetColor ();
         }
      }
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
