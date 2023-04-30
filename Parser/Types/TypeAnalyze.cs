// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// TypeAnalyze.cs ~ Type checking, type coercion
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;
using static NType;
using static Token.E;

public class TypeAnalyze : Visitor<NType> {
   public TypeAnalyze () {
      mSymbols = SymTable.Root;
   }
   SymTable mSymbols;

   #region Declarations ------------------------------------
   public override NType Visit (NProgram p) 
      => Visit (p.Block);
   
   public override NType Visit (NBlock b) {
      using var _ = new SymScope (this);
      Visit (b.Declarations); Visit (b.Body);
      return Void;
   }

   public override NType Visit (NDeclarations d) {
      Visit (d.Vars); return Visit (d.Funcs);
   }

   public override NType Visit (NVarDecl d) {
      ValidateSymbol (d.Name);
      mSymbols.Vars.Add (d);
      return d.Type;
   }

   public override NType Visit (NFnDecl f) {
      ValidateSymbol (f.Name);
      mSymbols.Funcs.Add (f);
      // Define a scope for the function parameters.
      using var _ = new SymScope (this);
      Visit (f.Params);
      // Assume all function parameters are initialized.
      f.Params.ForEach (x => x.SetFlag (EFlag.Initialized));
      // Add a temporary variable to handle Function return statement.
      if (f.Return != Void) mSymbols.Vars.Add (new (f.Name, f.Return));
      f.Body?.Accept (this);
      return f.Return;
   }
   #endregion

   #region Statements --------------------------------------
   public override NType Visit (NCompoundStmt b)
      => Visit (b.Stmts);

   public override NType Visit (NAssignStmt a) {
      if (mSymbols.Find (a.Name.Text) is not NVarDecl v || v.IsConstant ())
         throw new ParseException (a.Name, "Unknown variable");
      a.Expr.Accept (this);
      a.Expr = AddTypeCast (a.Name, a.Expr, v.Type);
      v.SetFlag (EFlag.Initialized);
      return v.Type;
   }
   
   NExpr AddTypeCast (Token token, NExpr source, NType target) {
      if (source.Type == target) return source;
      bool valid = (source.Type, target) switch {
         (Int, Real) or (Char, Int) or (Char, String) => true,
         _ => false
      };
      if (!valid) throw new ParseException (token, "Invalid type");
      return new NTypeCast (source) { Type = target };
   }

   public override NType Visit (NWriteStmt w)
      => Visit (w.Exprs);

   public override NType Visit (NIfStmt f) {
      f.Condition.Accept (this);
      f.IfPart.Accept (this); f.ElsePart?.Accept (this);
      return Void;
   }

   public override NType Visit (NForStmt f) {
      using var _ = new SymScope (this);
      var loopVar = new NVarDecl (f.Var, f.Start.Accept (this)).SetFlag (EFlag.Initialized);
      mSymbols.Vars.Add (loopVar);
      f.End.Accept (this); f.Body.Accept (this);
      return Void;
   }

   public override NType Visit (NReadStmt r) {
      foreach (var t in r.Vars) {
         if (mSymbols.Find (t.Text) is NVarDecl v && !v.IsConstant ()) {
            v.SetFlag (EFlag.Initialized);
            continue;
         }
         throw new ParseException (t, "Unknown variable");
      }
      return Void;
   }

   public override NType Visit (NWhileStmt w) {
      w.Condition.Accept (this); w.Body.Accept (this);
      return Void; 
   }

   public override NType Visit (NRepeatStmt r) {
      Visit (r.Stmts); r.Condition.Accept (this);
      return Void;
   }

   public override NType Visit (NCallStmt c) => Visit (c.Name, c.Params);
   #endregion

   #region Expression --------------------------------------
   public override NType Visit (NLiteral t) {
      t.SetFlag (EFlag.Initialized);
      return t.Type = t.Value.GetLiteralType ();
   }

   public override NType Visit (NUnary u) 
      => u.Type = u.Expr.Accept (this);

   public override NType Visit (NBinary bin) {
      NType a = bin.Left.Accept (this), b = bin.Right.Accept (this);
      bin.Type = (bin.Op.Kind, a, b) switch {
         (ADD or SUB or MUL or DIV, Int or Real, Int or Real) when a == b => a,
         (ADD or SUB or MUL or DIV, Int or Real, Int or Real) => Real,
         (MOD, Int, Int) => Int,
         (ADD, String, _) => String, 
         (ADD, _, String) => String,
         (LT or LEQ or GT or GEQ, Int or Real, Int or Real) => Bool,
         (LT or LEQ or GT or GEQ, Int or Real or String or Char, Int or Real or String or Char) when a == b => Bool,
         (EQ or NEQ, _, _) when a == b => Bool,
         (EQ or NEQ, Int or Real, Int or Real) => Bool,
         (AND or OR, Int or Bool, Int or Bool) when a == b => a,
         _ => Error,
      };
      if (bin.Type == Error)
         throw new ParseException (bin.Op, "Invalid operands");
      var (acast, bcast) = (bin.Op.Kind, a, b) switch {
         (_, Int, Real) => (Real, Void),
         (_, Real, Int) => (Void, Real), 
         (_, String, not String) => (Void, String),
         (_, not String, String) => (String, Void),
         _ => (Void, Void)
      };
      if (acast != Void) bin.Left = new NTypeCast (bin.Left) { Type = acast };
      if (bcast != Void) bin.Right = new NTypeCast (bin.Right) { Type = bcast };
      return bin.Type;
   }

   public override NType Visit (NIdentifier d) {
      if (mSymbols.Find (d.Name.Text) is NVarDecl v) {
         if (v.Initialized ()) d.SetFlag (EFlag.Initialized);
         else throw new ParseException (d.Name, $"Using uninitialized variable '{d.Name.Text}'");
         return d.Type = v.Type;
      }
      throw new ParseException (d.Name, "Unknown variable");
   }

   public override NType Visit (NFnCall f) => f.Type = Visit (f.Name, f.Params);

   // Used by NFnCall and NFnCallStmt.
   NType Visit (Token name, NExpr[] args) {
      if (mSymbols.Find (name.Text, SymTable.EFind.Functions) is not NFnDecl d)
         throw new ParseException (name, "Unknown function");
      var expected = d.Params;
      if (expected.Length != args.Length)
         throw new ParseException (name, $"Wrong number of parameters specified for \"{d.Name.Text}\". Expected '{expected.Length}', found '{args.Length}'.");
      for (int i = 0; i < args.Length; i++) {
         var (var, exp) = (expected[i], args[i]);
         var (a, b) = (var.Type, exp.Accept (this));
         try {
            args[i] = AddTypeCast (name, exp, a);
         } catch (ParseException) {
            if (a != b) throw new ParseException (name, $"Incompatible type for parameter {i + 1}.  Expected '{a}', found '{b}'.");
            throw;
         }
      }
      return d.Return;
   }

   public override NType Visit (NTypeCast c) {
      c.Expr.Accept (this); return c.Type;
   }
   #endregion

   NType Visit (IEnumerable<Node> nodes) {
      foreach (var node in nodes) node.Accept (this);
      return Void;
   }

   void ValidateSymbol (Token name) {
      var node = mSymbols.Find (name.Text, recurse: false);
      if (node != null) {
         var what = node is NFnDecl ? "Function" : "Identifier";
         throw new ParseException (name, $"{what} with name '{name.Text}' already exists in current scope");
      }
   }

   #region Nested types ----------------
   // A Symbol Scope.
   class SymScope : IDisposable {
      public SymScope (TypeAnalyze owner) {
         Owner = owner;
         owner.mSymbols = new SymTable { Parent = owner.mSymbols };
      }

      public void Dispose () {
         Owner.mSymbols = Owner.mSymbols.Parent!;
      }
      TypeAnalyze Owner;
   }
   #endregion
}
