namespace PSI;

static public class Extensions {
   /// <summary>Clamps the given value so it lies with the range min..max</summary>
   public static T Clamp<T> (this T a, T min, T max) where T : IComparable<T> {
      if (a.CompareTo (min) < 0) return min;
      if (a.CompareTo (max) > 0) return max;
      return a;
   }

   public static void ForEach<T> (this T[]? array, Action<T> action) {
      if (array != null)
         foreach (var elem in array) action (elem);
   }

   /// <summary>Adds Quotes around a string</summary>
   public static string Quoted (this string s) => $"\"{s}\"";
}
