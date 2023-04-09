namespace PSI;

public class ExprGrapher : Visitor<int> {
   public ExprGrapher (string expression) => mExpression = expression;
   StringBuilder mSB = new ();
   readonly string mExpression;
   int mID = 0;

   public override int Visit (NLiteral lit) {
      mSB.AppendLine ($"id{++mID}[{lit.Value} : {lit.Type}]");
      return mID;
   }

   public override int Visit (NIdentifier ident) {
      mSB.AppendLine ($"id{++mID}[{ident.Name.Text} : {ident.Type}]");
      return mID;
   }

   public override int Visit (NUnary unary) {
      int id = unary.Expr.Accept (this);
      mSB.AppendLine ($"id{++mID}([{unary.Op.Text} : {unary.Type}]); id{mID} --> id{id}");
      return mID;
   }

   public override int Visit (NBinary binary) {
      int a = binary.Left.Accept (this), b = binary.Right.Accept (this);
      mSB.AppendLine ($"id{++mID}([{binary.Op.Text} : {binary.Type}]); id{mID} --> id{a}; id{mID} --> id{b}");
      return mID; 
   }

   public void SaveTo (string file) {
      string text = template.Replace ("[EXPR]", mExpression)
                            .Replace ("[GRAPH]", mSB.ToString ());
      File.WriteAllText (file, text);
   }
   static string template = """
      <!DOCTYPE html>
      <head><meta charset="utf-8"></head>
      <body>
        Graph of [EXPR]
        <pre class="mermaid">
          graph TD
          [GRAPH]
        </pre> 
        <script type="module">
          import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.esm.min.mjs';
          mermaid.initialize({ startOnLoad: true });
        </script>  
      </body>
      """;
}