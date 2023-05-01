// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Types.cs ~ Represents Types, Context (symbol table) classes
// ─────────────────────────────────────────────────────────────────────────────
using PSILib;
namespace PSI;
using static NType;

// The data-type at any NExpr node
public enum NType { Unknown, Int, Real, Bool, String, Char, Error, Void }

public class SymTable {
   public Dictionary<string, NDecl> Entries = new (StringComparer.OrdinalIgnoreCase);
   public SymTable? Parent;

   public void Add (NDecl d) {
      try { Entries.Add (d.Name.Text, d); } 
      catch { throw new ParseException (d.Name, "Duplicate identifier"); }
   }

   public NDecl? Find (string name) {
      if (Entries.TryGetValue (name, out var entry)) return entry;
      return Parent?.Find (name);
   }

   // Contains symbols for the PSILib runtime library
   public static SymTable Root {
      get {
         if (mRoot == null) {
            mRoot = new ();
            Type type = typeof (Lib);
            foreach (var pi in type.GetProperties ()) 
               mRoot.Entries.Add (pi.Name, new NVarDecl (new Token (pi.Name), mMap[pi.PropertyType]));
            foreach (var mi in type.GetMethods ()) {
               if (mi.Name.StartsWith ("get_") || mi.Name.StartsWith ("set_")) continue;
               if (!mi.IsStatic) continue;
               var args = mi.GetParameters ().Select (a => new NVarDecl (new Token (a.Name!), mMap[a.ParameterType])).ToArray (); ;
               mRoot.Entries.Add (mi.Name, new NFnDecl (new Token (mi.Name), args, mMap[mi.ReturnType], null));
            }
         }
         return mRoot;
      }
   }
   static SymTable? mRoot;
   static Dictionary<Type, NType> mMap = new () {
      [typeof (int)] = Int, [typeof (double)] = Real, [typeof (string)] = String,
      [typeof (bool)] = Bool, [typeof (char)] = Char, [typeof (void)] = Void,
   };
}