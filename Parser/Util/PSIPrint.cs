// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// PSIPrint.cs ~ Prints a PSI syntax tree in Pascal format
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;

public class PSIPrint : Visitor<StringBuilder> {
   public override StringBuilder Visit (NProgram p) {
      Write ($"program {p.Name}; ");
      Visit (p.Block);
      return Write (".");
   }

   public override StringBuilder Visit (NBlock b) 
      => Visit (b.Declarations, b.Body);

   public override StringBuilder Visit (NDeclarations d) {
      if (d.Vars.Length > 0) {
         NWrite ("var"); N++;
         foreach (var g in d.Vars.GroupBy (a => a.Type))
            NWrite ($"{g.Select (a => a.Name).ToCSV ()} : {g.Key};");
         N--;
      }
      foreach (var f in d.Funcs) f.Accept (this);
      return S;
   }

   public override StringBuilder Visit (NVarDecl d)
      => NWrite ($"{d.Name} : {d.Type}");

   public override StringBuilder Visit (NFnDecl f) {
      NWrite (f.Return == NType.Void ? "procedure " : "function ");
      Write ($"{f.Name.Text} (");
      for (int i = 0; i < f.Params.Length; i++) {
         if (i > 0) Write (", ");
         Write ($"{f.Params[i].Name.Text}: {f.Params[i].Type}");
      }
      Write (")");
      if (f.Return != NType.Void) Write ($": {f.Return}");
      Write (";");
      if (f.Body != null) {
         Visit (f.Body);
         Write (";");
      }
      return S;
   }

   public override StringBuilder Visit (NTypeCast c) {
      Write ($"({c.Type})"); return c.Expr.Accept (this); 
   }

   public override StringBuilder Visit (NCompoundStmt b) {
      NWrite ("begin"); N++; Visit (b.Stmts); N--; return NWrite ("end"); 
   }

   public override StringBuilder Visit (NIfStmt f) {
      NWrite ("if "); f.Condition.Accept (this); Write (" then");
      N++; Visit (f.IfPart); N--;
      if (f.ElsePart != null) {
         NWrite ("else");
         N++; Visit (f.ElsePart); N--;
      }
      return S;
   }

   public override StringBuilder Visit (NForStmt f) {
      NWrite ($"for {f.Var.Text} := ");
      f.Start.Accept (this); Write (f.Ascending ? " to " : " downto ");
      f.End.Accept (this); Write (" do ");
      N++; f.Body.Accept (this); N--;
      return S;
   }

   public override StringBuilder Visit (NAssignStmt a) {
      NWrite ($"{a.Name} := "); a.Expr.Accept (this); return Write (";");
   }

   public override StringBuilder Visit (NWriteStmt w) {
      NWrite (w.NewLine ? "WriteLn (" : "Write (");
      for (int i = 0; i < w.Exprs.Length; i++) {
         if (i > 0) Write (", ");
         w.Exprs[i].Accept (this);
      }
      return Write (");");
   }

   public override StringBuilder Visit (NReadStmt r) {
      NWrite ("Read (");
      for (int i = 0; i < r.Vars.Length; i++) {
         if (i > 0) Write (", ");
         Write (r.Vars[i].Text); 
      }
      return Write (");");
   }

   public override StringBuilder Visit (NWhileStmt w) {
      NWrite ("while "); w.Condition.Accept (this); Write (" do ");
      N++; w.Body.Accept (this); N--;
      if (w.Body is NCompoundStmt) Write (";");
      return S;
   }

   public override StringBuilder Visit (NRepeatStmt r) {
      NWrite ("repeat ");
      N++; r.Stmts.ForEach (a => a.Accept (this)); N--;
      NWrite ("until "); 
      r.Condition.Accept (this); Write (";");
      return S;
   }

   public override StringBuilder Visit (NLiteral t)
      => Write (t.Value.ToString ());

   public override StringBuilder Visit (NIdentifier d)
      => Write (d.Name.Text);

   public override StringBuilder Visit (NUnary u) {
      Write (u.Op.Text); return u.Expr.Accept (this);
   }

   public override StringBuilder Visit (NBinary b) {
      Write ("("); b.Left.Accept (this); Write ($" {b.Op.Text} ");
      b.Right.Accept (this); return Write (")");
   }

   public override StringBuilder Visit (NFnCall f) {
      Write ($"{f.Name} (");
      for (int i = 0; i < f.Params.Length; i++) {
         if (i > 0) Write (", "); f.Params[i].Accept (this);
      }
      return Write (")");
   }

   public override StringBuilder Visit (NCallStmt c) {
      NWrite ($"{c.Name} (");
      for (int i = 0; i < c.Params.Length; i++) {
         if (i > 0) Write (", "); c.Params[i].Accept (this);
      }
      return Write (");");
   }

   StringBuilder Visit (params Node[] nodes) {
      nodes.ForEach (a => a.Accept (this));
      return S;
   }

   // Writes in a new line
   StringBuilder NWrite (string txt) 
      => Write ($"\n{new string (' ', N * 3)}{txt}");
   int N;   // Indent level

   // Continue writing on the same line
   StringBuilder Write (string txt) {
      Console.Write (txt);
      S.Append (txt);
      return S;
   }

   readonly StringBuilder S = new ();
}