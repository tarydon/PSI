namespace PSI;

public class ExprGrapher : Visitor<int> {
   public ExprGrapher (string expression) 
      => mExpression = expression;

   public override int Visit (NLiteral lit) 
      => NewNode ($"[{lit.Value} : {lit.Type}]");

   public override int Visit (NIdentifier ident) 
      => NewNode ($"[{ident.Name.Text} : {ident.Type}]");

   public override int Visit (NUnary unary) {
      int a = unary.Expr.Accept (this);
      int id = NewNode ($"([{unary.Op.Text} : {unary.Type}])");
      mSB.AppendLine ($"id{mID} --> id{a}");
      return id;
   }

   public override int Visit (NBinary binary) {
      int a = binary.Left.Accept (this), b = binary.Right.Accept (this);
      int id = NewNode ($"([{binary.Op.Text} : {binary.Type}])");
      mSB.AppendLine ($"id{mID} --> id{a}; id{mID} --> id{b}");
      return id; 
   }

   public override int Visit (NFnCall fn) {
      var args = fn.Params.Select (a => a.Accept (this));
      int id = NewNode ($"([{fn.Name.Text} : {fn.Type}])");
      foreach (var a in args) mSB.AppendLine ($"id{id} --> id{a}");
      return id; 
   }

   public void SaveTo (string file) {
      string text = $$"""
         <!DOCTYPE html>
         <head><meta charset="utf-8"></head>
         <body>
           Graph of {{mExpression}}
           <pre class="mermaid">
             graph TD
             {{mSB}}
           </pre> 
           <script type="module">
             import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.esm.min.mjs';
             mermaid.initialize({ startOnLoad: true });
           </script>  
         </body>
         """;
      File.WriteAllText (file, text);
   }

   int NewNode (string text) {
      mSB.AppendLine ($"id{++mID}{text}");
      return mID;
   }

   readonly StringBuilder mSB = new ();
   readonly string mExpression;
   int mID = 0;
}