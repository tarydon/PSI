namespace PSITest;

[AttributeUsage (AttributeTargets.Method)]
class TestAttribute : Attribute {
   public TestAttribute (int id, string name) => (Id, Name) = (id, name);
   public int Id { get; }
   public string Name { get; }
}

[AttributeUsage (AttributeTargets.Class)]
class TestFixtureAttribute : Attribute {
   public TestFixtureAttribute (int id, string name) => (Id, Name) = (id, name);
   public int Id { get; }
   public string Name { get; }
}

class Program {
   static void Main (string[] args) {
   }
}
