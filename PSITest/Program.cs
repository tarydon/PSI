using System.Reflection;

namespace PSITest;

[AttributeUsage (AttributeTargets.Method)]
class TestAttribute : Attribute {
   public TestAttribute (int id, string name) => (Id, Description) = (id, name);
   public int Id { get; }
   public string Description { get; }
}

[AttributeUsage (AttributeTargets.Class)]
class TestFixtureAttribute : Attribute {
   public TestFixtureAttribute (int id, string name) => (Id, Description) = (id, name);
   public int Id { get; }
   public string Description { get; }
}

class Program {
   static void Main (string[] args) {
      Assembly assy = Assembly.GetExecutingAssembly ();
      var bindir = Path.GetDirectoryName (assy.Location);
      Root = Path.GetFullPath (bindir + "/../TData").Replace ('\\', '/');

      Runner r = new Runner ();
      r.GatherTests (assy);
      r.RunTests ();
      CoverLib.HitCounter.Save ($"{bindir}/hits.txt");
   }

   static public string Root = "";
}
