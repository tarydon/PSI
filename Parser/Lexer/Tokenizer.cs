// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Tokenizer.cs ~ Converts text to a stream of Tokens
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;
using static Token.E;

// Converts a stream of text to PSI Tokens
public class Tokenizer {
   public Tokenizer (string text) => mText = text + " ";
   readonly string mText;
   int mN, mLine = 1, mLineStart = -1;

   /// <summary>The source text, split into lines</summary>
   public string[] Lines => mLines ??= mText.Split ('\n').Select (a => a.TrimEnd ()).ToArray ();
   string[]? mLines;

   /// <summary>The source file from which the code has been read in</summary>
   public string FileName => "untitled.pas";

   // Returns the next token (returns EOF token if no more are left)
   public Token Next () {
      for (; ; ) {
         if (mN >= mText.Length) return Make (EOF, mN);
         char ch = mText[mN];
         if (char.IsWhiteSpace (ch)) {
            if (ch == '\n') { mLineStart = mN; mLine++; }
            mN++; continue; 
         }
         if (char.IsLetter (ch)) return IdentifierOrKeyword ();
         if (char.IsDigit (ch)) return Number ();
         if (ch == '"') return String ();
         if (ch == '\'') return Char ();
         if (ch == '{') {
            int n = mText.IndexOf ('}', mN + 1);
            if (n == -1) return Make (ERROR, "Unterminated comment", mN);
            mN = n + 1; continue; 
         }
         return PunctuationOrOperator ();
      }
   }

   // Implementation ------------------------------------------------
   // If we see a numeric digit, construct a number token
   Token Number () {
      var m = sExpr.Match (mText[mN..]);
      if (m.Success) {
         int start = mN;
         string text = mText[mN..(mN + m.Length)];
         mN += m.Length;
         if (text.Contains ('.') || text.Contains ('e') || text.Contains ('E'))
            return Make (L_REAL, text, start);
         else
            return Make (L_INTEGER, text, start);
      }
      return Make (ERROR, "Invalid number", mN);
   }
   static readonly Regex sExpr = new (@"^[+-]?\d+(\.\d+)?([Ee][+-]?\d+)?", RegexOptions.Compiled | RegexOptions.CultureInvariant);

   // If we see an open " then we construct a string token
   Token String () {
      int start = mN++;
      while (mN < mText.Length && mText[mN++] != '"') { }
      return Make (L_STRING, mText[(start + 1)..(mN - 1)], start);
   }

   // If we see an open ' then we construct a char token
   Token Char () {
      int start = mN++;
      while (mN < mText.Length && mText[mN++] != '\'') { }
      string text = mText[(start + 1)..(mN - 1)];
      if (text.Length != 1) return Make (ERROR, "Invalid character", start);
      return Make (L_CHAR, text, start);
   }

   // If we see an alpha character, construct an identifier or keyword
   Token IdentifierOrKeyword () {
      int start = mN;
      while (char.IsLetterOrDigit (mText[++mN])) { }
      string s = mText[start..mN];
      if (Enum.TryParse (s, true, out Token.E kind) && kind < _ENDKEYWORDS)
         return Make (kind, s, start);
      return s switch { 
         "true" or "false" => Make (L_BOOLEAN, s, start), 
         _ => Make (IDENT, s, start) 
      };
   }

   // Construct a punctuation or an operator token
   Token PunctuationOrOperator () {
      foreach (var (kind, text) in Token.Match) {
         int n = text.Length;
         if (mN + n < mText.Length && text == mText[mN..(mN + n)]) {
            int start = mN; mN += n; 
            return Make (kind, text, start);
         }
      }
      return Make (ERROR, $"Unexpected '{mText[mN]}'", mN);
   }

   // Helper functions 
   Token Make (Token.E kind, string text, int n) 
      => new (this, kind, text, mLine, n - mLineStart);
   Token Make (Token.E kind, int n)
      => Make (kind, "", n);
}
