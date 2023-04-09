namespace PSI;
using static Token.E;

// Converts a stream of text to PSI Tokens
public class Tokenizer {
   public Tokenizer (string text) => mText = text + " ";
   readonly string mText;
   int mN;

   // Returns the next token (returns EOF token if no more are left)
   public Token Next () {
      for (; ; ) {
         if (mN >= mText.Length) return new (EOF, "");
         char ch = mText[mN];
         if (char.IsWhiteSpace (ch)) { mN++; continue; }
         if (char.IsLetter (ch)) return IdentifierOrKeyword ();
         if (char.IsDigit (ch)) return Number ();
         if (ch == '"') return String ();
         if (ch == '\'') return Char ();
         return PunctuationOrOperator ();
      }
   }

   // Implementation ------------------------------------------------
   // If we see a numeric digit, construct a number token
   Token Number () {
      var m = sExpr.Match (mText[mN..]);
      if (m.Success) {
         string text = mText[mN..(mN + m.Length)];
         mN += m.Length;
         if (text.Contains ('.') || text.Contains ('e') || text.Contains ('E'))
            return new (REAL, text);
         else
            return new (INTEGER, text);
      }
      return new (ERROR, "");
   }
   static Regex sExpr = new (@"^[+-]?\d+(\.\d+)?([Ee][+-]?\d+)?", RegexOptions.Compiled | RegexOptions.CultureInvariant);

   // If we see an open " then we construct a string token
   Token String () {
      int start = mN++;
      while (mN < mText.Length && mText[mN++] != '"') { }
      return new (STRING, mText[(start + 1)..(mN - 1)]);
   }

   // If we see an open ' then we construct a char token
   Token Char () {
      int start = mN++;
      while (mN < mText.Length && mText[mN++] != '\'') { }
      string text = mText[(start + 1)..(mN - 1)];
      if (text.Length != 1) return new (ERROR, "");
      return new (CHAR, text);
   }

   // If we see an alpha character, construct an identifier or keyword
   Token IdentifierOrKeyword () {
      int start = mN;
      while (char.IsLetterOrDigit (mText[++mN])) { }
      string s = mText[start..mN];
      if (s == s.ToLower () && Enum.TryParse (s, true, out Token.E kind) && kind < _ENDKEYWORDS)
         return new (kind, s);
      return s switch { "true" or "false" => new (BOOLEAN, s), _ => new (IDENT, s) };
   }

   // Construct a punctuation or an operator token
   Token PunctuationOrOperator () {
      foreach (var (kind, text) in Token.Match) {
         int n = text.Length;
         if (mN + n < mText.Length && text == mText[mN..(mN + n)]) {
            mN += n;
            return new (kind, text);
         }
      }
      return new (ERROR, "");
   }
}