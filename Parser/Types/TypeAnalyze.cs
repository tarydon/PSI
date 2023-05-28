// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// TypeAnalyze.cs ~ Type checking, type coercion
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;
using static NType;
using static Token.E;

class TypeAnalyze : Visitor<NType> {
   #region Declarations ------------------------------------
   public override NType Visit (NProgram p) 
      => Visit (p.Block);
   
   public override NType Visit (NBlock b) {
      mSymbols = new SymTable { Parent = mSymbols };
      Visit (b.Declarations); Visit (b.Body);
      mSymbols = mSymbols.Parent;
      return Void;
   }

   public override NType Visit (NDeclarations d) {
      Visit (d.Consts); Visit (d.Vars); 
      return Visit (d.Funcs);
   }

   public override NType Visit (NConstDecl c) {
      mSymbols.Add (c);
      return c.Value.Accept (this);
   }

   public override NType Visit (NVarDecl d) {
      mSymbols.Add (d);
      return d.Type;
   }

   public override NType Visit (NFnDecl f) {
      mSymbols.Add (f);
      mSymbols = new SymTable { Parent = mSymbols };
      f.Params.ForEach (v => { mSymbols.Add (v); v.Assigned = true; });
      if (f.Block != null) {
         f.Block.Accept (this);
         if (!f.Assigned && f.Return != Void) 
            Fatal (f.Block.Body.Token, "Function return value not set");
      }
      mSymbols = mSymbols.Parent;
      return f.Return;
   }
   #endregion

   #region Statements --------------------------------------
   public override NType Visit (NCompoundStmt b)
      => Visit (b.Stmts);

   public override NType Visit (NAssignStmt a) {
      a.Expr.Accept (this);
      NType type;
      switch (mSymbols.Find (a.Name)) {
         case NVarDecl v: type = v.Type; v.Assigned = true; break;
         case NFnDecl f: type = f.Return; f.Assigned = true; break;
         case NConstDecl:  throw new ParseException (a.Name, "Cannot assign to a constant");
         default: throw new ParseException (a.Name, "Unknown variable");
      }
      a.Expr = AddTypeCast (a.Expr, type);
      return type;
   }
   
   NExpr AddTypeCast (NExpr source, NType target) {
      source.Accept (this);
      if (source.Type == target) return source;
      bool valid = (source.Type, target) switch {
         (Integer, Real) or (Char, Integer) or (Char, String) => true,
         _ => false
      };
      if (!valid) {
         string error = $"Type error: expecting {target.ToString ().ToLower ()}, found {source.Type.ToString ().ToLower ()}";
         throw new ParseException (source.Token, error);
      }
      return new NTypeCast (source) { Type = target };
   }

   public override NType Visit (NWriteStmt w)
      => Visit (w.Exprs);

   public override NType Visit (NIfStmt f) {
      f.Condition = AddTypeCast (f.Condition, Bool);
      f.IfPart.Accept (this); f.ElsePart?.Accept (this);
      return Void;
   }

   public override NType Visit (NForStmt f) {
      var v = ExpectVar (f.Var);
      if (v.Type != Integer) Fatal (f.Var, "For loop variable must be an integer");
      v.Assigned = true; 
      f.Start = AddTypeCast (f.Start, v.Type);
      f.End = AddTypeCast (f.End, v.Type);
      f.Body.Accept (this);
      return Void;
   }

   public override NType Visit (NReadStmt r) {
      foreach (var name in r.Vars) 
         ExpectVar (name).Assigned = true; 
      return Void;
   }

   NVarDecl ExpectVar (Token name) {
      switch (mSymbols.Find (name)) {
         case NVarDecl v: return v;
         case NConstDecl: throw new ParseException (name, "Expecting var, found const");
         default: throw new ParseException (name, "Unknown variable");
      }
   }

   public override NType Visit (NWhileStmt w) {
      w.Condition = AddTypeCast (w.Condition, Bool);
      w.Body.Accept (this);
      return Void; 
   }

   public override NType Visit (NRepeatStmt r) {
      Visit (r.Stmts);
      r.Condition = AddTypeCast (r.Condition, Bool);
      return Void;
   }

   public override NType Visit (NCallStmt c) {
      CheckFunctionCall (c.Name, c.Params);
      return Void; 
   }
   #endregion

   #region Expression --------------------------------------
   public override NType Visit (NLiteral t) {
      t.Type = t.Value.Kind switch {
         L_INTEGER => Integer, L_REAL => Real, L_BOOLEAN => Bool, L_STRING => String,
         L_CHAR => Char, _ => Error,
      };
      return t.Type;
   }

   public override NType Visit (NUnary u) 
      => u.Expr.Accept (this);

   public override NType Visit (NBinary bin) {
      NType a = bin.Left.Accept (this), b = bin.Right.Accept (this);
      bin.Type = (bin.Op.Kind, a, b) switch {
         (ADD or SUB or MUL or DIV, Integer or Real, Integer or Real) when a == b => a,
         (ADD or SUB or MUL or DIV, Integer or Real, Integer or Real) => Real,
         (MOD, Integer, Integer) => Integer,
         (ADD, String, _) => String, 
         (ADD, _, String) => String,
         (LT or LEQ or GT or GEQ, Integer or Real, Integer or Real) => Bool,
         (LT or LEQ or GT or GEQ, Integer or Real or String or Char, Integer or Real or String or Char) when a == b => Bool,
         (EQ or NEQ, _, _) when a == b => Bool,
         (EQ or NEQ, Integer or Real, Integer or Real) => Bool,
         (AND or OR, Integer or Bool, Integer or Bool) when a == b => a,
         _ => Error,
      };
      if (bin.Type == Error)
         throw new ParseException (bin.Op, "Invalid operands");
      var (acast, bcast) = (bin.Op.Kind, a, b) switch {
         (_, Integer, Real) => (Real, Void),
         (_, Real, Integer) => (Void, Real), 
         (_, String, not String) => (Void, String),
         (_, not String, String) => (String, Void),
         _ => (Void, Void)
      };
      if (acast != Void) bin.Left = new NTypeCast (bin.Left) { Type = acast };
      if (bcast != Void) bin.Right = new NTypeCast (bin.Right) { Type = bcast };
      return bin.Type;
   }

   public override NType Visit (NIdentifier d) {
      switch (mSymbols.Find (d.Name)) {
         case NVarDecl v:
            if (!v.Assigned) Fatal (d.Name, $"Variable {d.Name} not initialized");
            d.Type = v.Type;
            break;
         case NConstDecl c: 
            d.Type = c.Value.Type; 
            break;
         default:  
            throw new ParseException (d.Name, "Unknown variable");
      }
      return d.Type;
   }

   public override NType Visit (NFnCall f) 
      => f.Type = CheckFunctionCall (f.Name, f.Params);

   // Helper used to check NFnCall and NCallStmt
   NType CheckFunctionCall (Token name, NExpr[] args) {
      var fn = mSymbols.Find (name) as NFnDecl;
      if (fn == null) Fatal (name, "Unknown function / procedure");
      int a = fn.Params.Length, b = args.Length;
      if (a != b) Fatal (name, $"{fn.Name} needs {a} parameters, found {b}");
      for (int i = 0; i < a; i++) 
         args[i] = AddTypeCast (args[i], fn.Params[i].Type);
      return fn.Return;
   }

   public override NType Visit (NTypeCast c) {
      c.Expr.Accept (this); return c.Type;
   }
   #endregion

   [DoesNotReturn]
   void Fatal (Token token, string message)
      => throw new ParseException (token, message);

   NType Visit (IEnumerable<Node> nodes) {
      foreach (var node in nodes) node.Accept (this);
      return Void;
   }

   SymTable mSymbols = SymTable.Root;
}
