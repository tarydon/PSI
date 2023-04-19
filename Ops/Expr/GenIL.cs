namespace PSI;

using static Token.E;
using static NType;
using System.Diagnostics;

// An basic IL code generator, implemented using the Visitor pattern
public class ExprILGen : Visitor<StringBuilder> {
   public ExprILGen (Dictionary<string, double> vars) { mVars = vars; }
   Dictionary<string, double> mVars;
   HashSet<string> mLocals = new ();
   StringBuilder mSB = new ();
   int mID = 0;

   public override StringBuilder Visit (NLiteral literal) {
      string code = literal.Type switch {
         Int => "ldc.i4",
         Real => "ldc.r8",
         String => "ldstr",
         Boolean or Char => "ldc.i4.s",
         _ => "ldc.i4"
      };
      return Append ($"{code} {literal.Value.Text}");
   }

   public override StringBuilder Visit (NIdentifier identifier) { 
      if (!mVars.TryGetValue (identifier.Name.Text, out var value)) throw new KeyNotFoundException ();
      if (mLocals.Add ($"float64 {identifier.Name.Text}")) {
         Append ($"ldc.r8 {value}");
         Append ($"stloc {identifier.Name.Text}");
      }
      return Append ($"ldloc {identifier.Name.Text}");
   }

   public override StringBuilder Visit (NCast cast) {
      cast.Expr.Accept (this);
      if (cast.Type != cast.Expr.Type) Append ($"conv.r8");
      return mSB;
   }

   public override StringBuilder Visit (NUnary unary) {
      unary.Expr.Accept (this);
      if (unary.Op.Kind == SUB) Append ($"neg");
      return mSB;
   }

   public override StringBuilder Visit (NBinary binary) {
      binary.Left.Accept (this); binary.Right.Accept (this);
      return Append ($"{binary.Op.Kind.ToString ().ToLower ()}");
   }

   public override StringBuilder Visit (NFnCall fn) {
      var args = fn.Params.Select (x => x.Accept (this)).ToArray ();
      var name = fn.Name.Text.ToLower ();
      switch (name) {
         case "sin": case "cos": case "tan":
            D2R ();
            MathD (name);            
            break;

         case "asin": case "acos": case "atan":
            MathD (name);
            R2D ();
            break;

         case "log": case "exp":
         case "abs":
            MathD (name);
            break;

         case "atan2":
            MathD2 (name);
            R2D ();
            break;

         case "round":
            if (args.Length == 1) MathD (name);
            else MathDN (name);
            break;

         case "out":
            CallStatic ("void [System.Console]System.Console::WriteLine (float64)");
            break;

         case "random": 
            Append ("newobj instance void [mscorlib]System.Random::.ctor ()");
            CallVirt ("int32 [mscorlib]System.Random::Next ()");
            break;
                  
         default: throw new NotImplementedException ();
      }
      return mSB;

      static string Capitalize (string str) => $"{char.ToUpper (str[0])}{str[1..]}";
      void D2R () => CallStatic ($"float64 Program::D2R (float64)");
      void R2D () => CallStatic ("float64 Program::R2D (float64)");
      void MathD (string name) => CallStatic ($"float64 [mscorlib]System.Math::{Capitalize (name)} (float64)");
      void MathD2 (string name) => CallStatic ($"float64 [mscorlib]System.Math::{Capitalize (name)} (float64, float64)");
      void MathDN (string name) => CallStatic ($"float64 [mscorlib]System.Math::{Capitalize (name)} (float64, int32)");
      void CallVirt (string method) => CallMethod (method, false, true);
      void CallStatic (string method) => CallMethod (method, true, false);
      void CallMethod (string method, bool isstatic, bool isvirtual) {
         string call = isvirtual ? "callvirt" : "call";
         if (!isstatic) call += " instance";
         Append ($"{call} {method}");
      }
   }

   StringBuilder Append (string line) => mSB.AppendLine ($"{Label} {line}");


   public void Save (string filename) {
      filename = Path.GetFullPath (filename);
      string name = Path.GetFileNameWithoutExtension (filename);
      string locals = mLocals.Count == 0 ? "" : $".locals ({string.Join ("\n", mLocals)})";
      string strasm = $$"""
         //.assembly extern System.Runtime { .ver 7:0:0:0 }
         //.assembly extern System.Console { .ver 7:0:0:0 }
         .assembly {{name}} { .ver 1:0:0:0 }
         .class public Program extends [System.Runtime]System.Object {
            .method public static void Main () {
               .entrypoint
               .maxstack 8
               {{locals}}
               {{mSB}}
               {{Label}} conv.r8
               {{Label}} call void [System.Console]System.Console::WriteLine (float64) 
               {{Label}} ret
            }

           .method public static float64 D2R (float64 f) {
               .maxstack 8
               ldarg.0
               ldc.r8 3.1415926535897931
               mul
               ldc.r8 180
               div
               ret
            }

            .method public static float64 R2D (float64 f) {
               .maxstack 8
               ldarg.0
               ldc.r8 180
               mul
               ldc.r8 3.1415926535897931
               div
               ret
            }
         }
         """;
      File.WriteAllText (filename, strasm);
   }

   [DebuggerBrowsableAttribute (DebuggerBrowsableState.Never)]
   string Label => $"IL{++mID:D3}:";
}