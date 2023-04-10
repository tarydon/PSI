namespace PSILib;

public static class Lib {
   #region Variables -------------------------------------------------
   /// <summary>Height of terminal window in characters</summary>
   public static int ScreenHeight => Console.WindowHeight;

   /// <summary>Width of terminal window in characters</summary>
   public static int ScreenWidth => Console.WindowWidth;
   #endregion

   #region Functions ------------------------------------------------
   /// <summary>Clear the terminal window</summary>
   public static void ClrScr () => Console.Clear ();

   /// <summary>Pauses for the given number of milliseconds</summary>
   public static void Delay (int ms) => Thread.Sleep (ms);

   /// <summary>Move to the position (x,y) in the terminal window. Upper left is (1, 1)</summary>
   public static void GotoXY (int x, int y)
      => (Console.CursorLeft, Console.CursorTop) = (x - 1, y - 1);

   /// <summary>Exit the program immediately</summary>
   public static void Halt () => Environment.Exit (-1);

   /// <summary>Returns true if c is a digit (0..9)</summary>
   public static bool IsDigit (char c) => char.IsDigit (c);

   /// <summary>Returns true if c is an uppercase or lowercase ASCII character</summary>
   public static bool IsLetter (char c) => char.IsAsciiLetter (c);

   /// <summary>Returns true if the user has pressed a key (which you can then retrieve with the ReadKey function) or false otherwise.</summary>
   public static bool KeyPressed () => Console.KeyAvailable;

   /// <summary>Returns the leftmost n characters of the given string</summary>
   public static string LeftStr (string s, int n) => s[..n];

   /// <summary>Returns the base-2 logarithm of x</summary>
   public static double Log2 (double x) => Math.Log2 (x);

   /// <summary>Returns the maximum of two values</summary>
   public static int Max (int a, int b) => Math.Max (a, b);
   /// <summary>Returns the maximum of two values</summary>
   public static double Max (double a, double b) => Math.Max (a, b);

   /// <summary>Returns the minimum of two values</summary>
   public static int Min (int a, int b) => Math.Min (a, b);
   /// <summary>Returns the minimum of two values</summary>
   public static double Min (double a, double b) => Math.Min (a, b);

   /// <summary>Reads a key from the user, without waiting for the user to press Enter afterwards</summary>
   public static char ReadKey () => Console.ReadKey ().KeyChar;

   /// <summary>Returns a copy of s where all occurances of olds have been replaced with news</summary>
   public static string ReplaceText (string s, string olds, string news) => s.Replace (olds, news);

   /// <summary>Returns the rightmost n characters of the given string</summary>
   public static string RightStr (string s, int n) => s[(s.Length - n)..];

   /// <summary>Returns the sign of f, which is -1, 0 or +1</summary>
   public static int Sign (double f) => Math.Sign (f);
   /// <summary>Returns the sign of f, which is -1, 0 or +1</summary>
   public static int Sign (int n) => Math.Sign (n);

   /// <summary>Return a copy of s in which the string t replaces the segment of length characters beginning at index start.</summary>
   /// For example, stuffString('one more time', 5, 4, 'last') is 'one last time'.
   public static string StuffString (string s, int start, int len, string t) => s[..start] + t + s[(start + len)..];
   #endregion
}
