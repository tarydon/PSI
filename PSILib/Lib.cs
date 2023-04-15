namespace PSILib;

public static class Lib {
   #region Variables -------------------------------------------------
   /// <summary>Returns the number of command line args</summary>
   public static int ParamCount => Environment.GetCommandLineArgs ().Length - 1;

   public static double Pi => Math.PI;

   /// <summary>Height of terminal window in characters</summary>
   public static int ScreenHeight => Console.WindowHeight;

   /// <summary>Width of terminal window in characters</summary>
   public static int ScreenWidth => Console.WindowWidth;
   #endregion

   #region Functions ------------------------------------------------
   /// <summary>Returns the absolute value of an integer</summary>
   public static int Abs (int a) => Math.Abs (a);
   /// <summary>Returns the abolute value of a double</summary>
   public static double Abs (double a) => Math.Abs (a);

   /// <summary>Converts an integer to a character</summary>
   public static char Chr (int n) => (char)n;

   /// <summary>Clear the terminal window</summary>
   public static void ClrScr () => Console.Clear ();

   /// <summary>Returns the cosine of a value in radians</summary>
   public static double Cos (double f) => Math.Cos (f);

   /// <summary>Pauses for the given number of milliseconds</summary>
   public static void Delay (int ms) => Thread.Sleep (ms);

   /// <summary>Returns e to the power d</summary>
   public static double Exp (double d) => Math.Exp (d);

   /// <summary>Returns the fractional part of the given double value</summary>
   public static double Frac (double f) => f - (int)f;

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

   /// <summary>Returns the length of a string</summary>
   public static int Length (string s) => s.Length;

   /// <summary>Returns the natural logarithm of the given value</summary>
   public static double Ln (double f) => Math.Log (f);

   /// <summary>Returns the base-2 logarithm of x</summary>
   public static double Log2 (double x) => Math.Log2 (x);

   /// <summary>Converts a character to lowercase</summary>
   public static char LowerCase (char ch) => char.ToLower (ch);
   /// <summary>Convers a string to lowercase</summary>
   public static string LowerCase (string s) => s.ToLower ();

   /// <summary>Returns the maximum of two values</summary>
   public static int Max (int a, int b) => Math.Max (a, b);
   /// <summary>Returns the maximum of two values</summary>
   public static double Max (double a, double b) => Math.Max (a, b);

   /// <summary>Returns the minimum of two values</summary>
   public static int Min (int a, int b) => Math.Min (a, b);
   /// <summary>Returns the minimum of two values</summary>
   public static double Min (double a, double b) => Math.Min (a, b);

   /// <summary>Returns the nth command line parameter (they are numbered starting from 1)</summary>
   public static string ParamStr (int n) => Environment.GetCommandLineArgs ()[n - 1];

   /// <summary>Returns the first index at which c occurs in s, or 0 if c is not in s</summary>
   public static int Pos (char c, string s) => s.IndexOf (c) + 1;

   /// <summary>Returns a random integer between 0 .. n-1</summary>
   public static int Random (int n) => System.Random.Shared.Next (n);

   /// <summary>Rounds a double to the nearest integer</summary>
   public static int Round (double f) => (int)Math.Round (f);

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

   /// <summary>Returns the sin of a value (in radians)</summary>
   public static double Sin (double f) => Math.Sin (f);

   /// <summary>Returns a string consisting of the character c repeated n times</summary>
   public static string StringOfChar (char c, int n) => new string (c, n);

   /// <summary>Returns the square root of a value</summary>
   public static double Sqrt (double f) => Math.Sqrt (f);

   /// <summary>Return a copy of s in which the string t replaces the segment of length characters beginning at index start.</summary>
   /// For example, stuffString('one more time', 5, 4, 'last') is 'one last time'.
   public static string StuffString (string s, int start, int len, string t) => s[..start] + t + s[(start + len)..];

   /// <summary>Converts a character to uppercase</summary>
   public static char UpperCase (char ch) => char.ToUpper (ch);
   /// <summary>Converts a string to uppercase</summary>
   public static string UpperCase (string s) => s.ToUpper ();

   /// <summary>Writes one or more values to standard output</summary>
   public static void Write (object[] args) {
      foreach (var arg in args) Console.Write (arg);
   }

   /// <summary>Writes one or more values to standard output, followed by a newline</summary>
   public static void WriteLn (object[] args) {
      foreach (var arg in args) Console.Write (arg);
      Console.WriteLine ();
   }
   #endregion
}
