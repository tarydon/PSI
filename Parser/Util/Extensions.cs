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
}
