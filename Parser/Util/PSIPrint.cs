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
      => Visit (b.Decls, b.Body);

   public override StringBuilder Visit (NDeclarations d) {
      if (d.Vars.Length > 0) {
         NWrite ("var"); N++;
         foreach (var g in d.Vars.GroupBy (a => a.Type))
            NWrite ($"{g.Select (a => a.Name).ToCSV ()} : {g.Key};");
         N--;
      }
      return d.ProcFns.Accept (this);
   }

   public override StringBuilder Visit (NProcFnDecls d) {
      foreach (var fn in d.Fns) fn.Accept (this);
      foreach (var proc in d.Procs) proc.Accept (this);
      return S;
   }

   public override StringBuilder Visit (NProcDecl d) {
      NWrite ($"\nprocedure {d.Name} "); d.Params.Accept (this); Write (";");
      return d.Block.Accept (this);
   }

   public override StringBuilder Visit (NFnDecl d) {
      NWrite ($"\nfunction {d.Name} "); d.Params.Accept (this); Write ($" : {d.Type};");
      return d.Block.Accept (this);
   }

   public override StringBuilder Visit (NParams p) {
      Write ("(");
      List<string> vals = new ();
      foreach (var g in p.Vars.GroupBy (a => a.Type))
         vals.Add ($"{g.Select (a => a.Name).ToCSV ()} : {g.Key}");
      Write (string.Join ("; ", vals));
      return Write (")");
   }

   public override StringBuilder Visit (NVarDecl d)
      => NWrite ($"{d.Name} : {d.Type}");

   public override StringBuilder Visit (NCompoundStmt b) {
      NWrite ("begin"); N++;  Visit (b.Stmts); N--; return NWrite ("end"); 
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
      NWrite ("read (");
      for (int i = 0; i < r.Idents.Length; i++) {
         if (i > 0) Write (", ");
         Write (r.Idents[i].Text);
      }
      return Write (");");
   }

   public override StringBuilder Visit (NCallStmt c) {
      NWrite (""); c.FnCall.Accept (this); Write (";");
      return S;
   }

   public override StringBuilder Visit (NWhileStmt w) {
      NWrite ("while "); w.Exp.Accept (this); Write (" do"); 
      return w.Stmt.Accept (this);
   }

   public override StringBuilder Visit (NIfStmt i) {
      NWrite ("if "); i.Exp.Accept (this); Write (" then"); N++;
      i.Stmt.Accept (this); N--;
      return S;
   }

   public override StringBuilder Visit (NElseStmt e) {
      NWrite ("else"); N++;
      e.Stmt.Accept (this); N--;
      return S;
   }

   public override StringBuilder Visit (NRepeatStmt r) {
      NWrite ("repeat"); N++;
      foreach (var stmt in r.Stmts) stmt.Accept (this); N--;
      NWrite ("until "); 
      r.Exp.Accept (this); Write (";");
      return S;
   }

   public override StringBuilder Visit (NForStmt f) {
      NWrite ($"for {f.Name} := "); f.StartExp.Accept (this);
      Write (f.IsTo ? " to " : " downto "); f.EndExp.Accept (this);
      Write (" do "); N++; f.Stmt.Accept (this); N-- ;
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