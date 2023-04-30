// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// Types.cs ~ Represents Types, Context (symbol table) classes
// ─────────────────────────────────────────────────────────────────────────────
using PSILib;
namespace PSI;
using static NType;

// The data-type at any NExpr node
public enum NType { Unknown, Int, Real, Bool, String, Char, Error, Void }

public class SymTable {
   public enum EFind { Vars = 1, Functions = 2, Everything = 3}
   public List<NVarDecl> Vars = new ();
   public List<NFnDecl> Funcs = new ();
   public SymTable? Parent;

   public Node? Find (string name, EFind what = EFind.Everything, bool recurse = true) {
      if (what.HasFlag (EFind.Vars)) {
         var node1 = Vars.FirstOrDefault (a => a.Name.Text.EqualsIC (name));
         if (node1 != null) return node1;
      }
      if (what.HasFlag (EFind.Functions)) {
         var node2 = Funcs.FirstOrDefault (a => a.Name.Text.EqualsIC (name));
         if (node2 != null) return node2;
      }
      if (recurse) return Parent?.Find (name, what);
      return null;
   }

   // Contains symbols for the PSILib runtime library
   public static SymTable Root {
      get {
         if (mRoot == null) {
            mRoot = new ();
            Type type = typeof (Lib);
            foreach (var pi in type.GetProperties ()) 
               mRoot.Vars.Add (new NVarDecl (new Token (pi.Name), mMap[pi.PropertyType]));
            foreach (var mi in type.GetMethods ()) {
               if (mi.Name.StartsWith ("get_") || mi.Name.StartsWith ("set_")) continue;
               if (!mi.IsStatic) continue;
               var args = mi.GetParameters ().Select (a => new NVarDecl (new Token (a.Name!), mMap[a.ParameterType])).ToArray (); ;
               mRoot.Funcs.Add (new NFnDecl (new Token (mi.Name), args, mMap[mi.ReturnType], null));
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