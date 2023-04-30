// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Extensions.cs ~ Utility classes, and extension methods
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;
using static Token.E;
using static NType;

public static class Extensions {
   /// <summary>Clamps the given value so it lies with the range min..max</summary>
   public static T Clamp<T> (this T a, T min, T max) where T : IComparable<T> {
      if (a.CompareTo (min) < 0) return min;
      if (a.CompareTo (max) > 0) return max;
      return a;
   }

   /// <summary>Compares this string with another, ignoring case</summary>
   public static bool EqualsIC (this string a, string b) 
      => a.Equals (b, StringComparison.OrdinalIgnoreCase);

   /// <summary>Invoke an action on all elements in an array</summary>
   public static void ForEach<T> (this T[]? array, Action<T> action) {
      if (array != null)
         foreach (var elem in array) action (elem);
   }

   /// <summary>Adds Quotes around a string</summary>
   public static string Quoted (this string s) => $"\"{s}\"";

   /// <summary>Convert a sequence of objects to a comma-delimited string</summary>
   public static string ToCSV (this IEnumerable<object> objs)
      => string.Join (", ", objs);

   public static T Visit<T> (this Visitor<T> visitor, IEnumerable<Node> nodes) {
      T result = default!;
      foreach (var node in nodes) result = node.Accept (visitor);
      return result;
   }

   public static T SetFlag<T> (this T node, EFlag flag, bool value = true) where T : Node {
      if (value) node.Flags |= flag; else node.Flags &= ~flag;
      return node;
   }

   public static NType GetLiteralType (this Token tLiteral)
      => tLiteral.Kind switch {
         L_INTEGER => Int, L_REAL => Real, L_BOOLEAN => Bool,
         L_STRING => String, L_CHAR => Char, _ => Error,
      };

   // Is this node initialized?
   public static bool Initialized (this Node node) => node.Flags.HasFlag (EFlag.Initialized);

   // Is this var a constant?
   public static bool IsConstant (this NVarDecl var) => var.Flags.HasFlag (EFlag.Const);
}

public class ParseException : Exception {
   public ParseException (string fname, string[] source, int line, int column, string message) : base (message)
      => (FileName, Code, Line, Column) = (fname, source, line, column);

   public ParseException (Token token, string message) : base (message)
      => (FileName, Code, Line, Column) = (token.Source!.FileName, token.Source.Lines, token.Line, token.Column);

   public void Print () {
      if (Code != null) {
         const int gutter = 5;
         var (lines, title) = (Code, $"File: {FileName}");
         Console.WriteLine (title);
         Console.WriteLine ("┬".PadLeft (gutter, '─').PadRight (title.Length, '─'));
         for (int i = Line - 2; i <= Line + 2; i++) {
            if (i < 1 || i > lines.Length) continue;
            Console.WriteLine ($"{i,gutter - 1}|{lines[i - 1]}");
            if (i == Line) {
               Console.ForegroundColor = ConsoleColor.Yellow;
               Console.WriteLine ("^".PadLeft (Column + gutter)); // Error pointer
               int totalWidth = Column + gutter + Message.Length / 2;
               Console.WriteLine (Message.PadLeft (totalWidth));
               Console.ResetColor ();
            }
         }
      } else {
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.WriteLine ($"At line {Line}, column {Column}: {Message}");
         Console.ResetColor ();
      }
   }

   public string FileName { get; }
   public string[] Code { get; }
   public int Line { get; }
   public int Column { get; }
}
