// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Types.cs ~ Represents Types, Context (symbol table) classes
// ─────────────────────────────────────────────────────────────────────────────
using PSILib;
namespace PSI;
using static NType;

// The data-type at any NExpr node
public enum NType { Unknown, Int, Real, Bool, String, Char, Error, Void }

// Declarations in context in this Block
public class Context {
   public List<Var> Vars = new ();
   public List<Func> Funcs = new ();
   public Context? Parent;

   // The current context (intialized with the Runtime Library to start with)
   public static Context Current {
      get {
         if (mCurrent == null) {
            var c = new Context ();
            var type = typeof (Lib);
            foreach (var pi in type.GetProperties ()) {
               Token name = new (null, Token.E.IDENT, pi.Name, 0, 0);
               c.Vars.Add (new (name, mTypeMap[pi.PropertyType]));
            }
            foreach (var m in type.GetMethods ()) {
               if (!m.IsStatic) continue;
               if (m.Name.StartsWith ("get_")) continue;
               if (m.Name.StartsWith ("set_")) continue; 
               Console.WriteLine (m);
               List<Var> pars = new ();
               var retType = mTypeMap[m.ReturnType];
               foreach (var p in m.GetParameters ()) {
                  Token name = new (null, Token.E.IDENT, p.Name!, 0, 0);
                  pars.Add (new Var (name, mTypeMap[p.ParameterType]));
               }
               Token mname = new (null, Token.E.IDENT, m.Name, 0, 0);
               c.Funcs.Add (new Func (mname, pars.ToArray (), retType));
            }
            mCurrent = c;
         }
         return mCurrent;
      }
   }
   static Context? mCurrent;

   static Dictionary<Type, NType> mTypeMap = new () { 
      [typeof (int)] = Int, [typeof (double)] = Real, [typeof (bool)] = Bool, 
      [typeof (string)] = String, [typeof (char)] = Char, [typeof(void)] = Void
   };

   public record Var (Token Name, NType Type);
   public record Func (Token Name, Var[] Params, NType Return);
}