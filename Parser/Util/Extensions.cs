// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Extensions.cs ~ Utility classes, and extension methods
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;

public static class Extensions {
   /// <summary>Clamps the given value so it lies with the range min..max</summary>
   public static T Clamp<T> (this T a, T min, T max) where T : IComparable<T> {
      if (a.CompareTo (min) < 0) return min;
      if (a.CompareTo (max) > 0) return max;
      return a;
   }

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
}

public class ParseException : Exception {
   public ParseException (string fname, string[] source, int line, int column, string message) : base (message)
      => (FileName, Code, Line, Column) = (fname, source, line, column);

   public void Print () {
      Console.WriteLine (FileName);
      Console.WriteLine (Rep ('\u2500', 4) + '\u252c' + Rep ('\u2500', FileName.Length - 5));
      for (int i = Line - 2; i <= Line + 2; i++) {
         if (i < 1 || i > Code.Length) continue;
         Console.WriteLine ($"{i,4}\u2502{Code[i - 1]}");
         if (i == Line) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine (Rep (' ', Column + 4) + '^');
            int left = (Column + 4 - Message.Length / 2).Clamp (1, Console.WindowWidth - 2 - Message.Length);
            Console.WriteLine (Rep (' ', left) + Message);
            Console.ResetColor ();
         }
      }

      // Helper ..................................
      static string Rep (char ch, int n) => new string (ch, n);
   }

   public string FileName { get; }
   public string[] Code { get; }
   public int Line { get; }
   public int Column { get; }
}