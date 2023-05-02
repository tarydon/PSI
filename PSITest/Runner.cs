using System.Reflection;
using static System.Console;
namespace PSITest;

// Represents a class that contains tests
class Fixture {
   public Fixture (Type type, TestFixtureAttribute tf) {
      Id = tf.Id; Description = tf.Description;
      foreach (var method in type.GetMethods (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)) {
         var ta = method.GetCustomAttribute<TestAttribute> ();
         if (ta == null) continue;
         if (method.GetParameters ().Length != 0)
            throw new Exception ($"Method {type.Name}.{method.Name} should take no parameters");
         Tests.Add (new Test (method, ta));
      }
      var cons = type.GetConstructor (new Type[0]);
      if (cons == null) 
         throw new Exception ($"Type {type.Name} does not have a default constructor.");
      Cons = cons;
   }

   public readonly int Id;
   public readonly string Description;
   public readonly List<Test> Tests = new ();
   public readonly ConstructorInfo Cons;
}

// Represents a method that implements a test
class Test {
   public Test (MethodInfo mi, TestAttribute ta) {
      Id = ta.Id; Description = ta.Description;
      Method = mi;
   }

   public readonly int Id;
   public readonly string Description;
   public readonly MethodInfo Method;
}

class Runner {
   public void GatherTests (Assembly assy) {
      foreach (var t in assy.GetTypes ()) {
         var tf = t.GetCustomAttribute<TestFixtureAttribute> ();
         if (tf == null) continue;
         Fixtures.Add (new Fixture (t, tf));
      }
   }

   public List<Fixture> Fixtures = new ();

   public void RunTests () {
      int dx = WindowWidth - 1;
      int cTests = 0, cFailed = 0, cCrash = 0;
      var start = DateTime.Now;

      foreach (var fixture in Fixtures) {
         var fixtureObj = fixture.Cons.Invoke (null);
         BackgroundColor = ConsoleColor.DarkBlue;
         ForegroundColor = ConsoleColor.Gray;
         WriteLine ($"{fixture.Id} {fixture.Description}".PadRight (dx));
         ResetColor ();
         foreach (var test in fixture.Tests) {
            cTests++;
            string s = $"{test.Id} {test.Description} ";
            Write (s);
            ConsoleColor color = ConsoleColor.Green;
            string status = " pass";
            Exception? except = null;

            try {
               test.Method.Invoke (fixtureObj, null);
            } catch (TargetInvocationException te) {
               if (te.InnerException is TestFailException tf) {
                  cFailed++;
                  status = " FAIL";
                  color = ConsoleColor.Red;
               } else {
                  cCrash++;
                  status = "CRASH";
                  color = ConsoleColor.Red;
               }
               except = te.InnerException;
            }
            Console.Write (new string ('.', dx - s.Length - 5));

            ForegroundColor = color;
            Console.WriteLine (status);
            if (except != null) {
               ForegroundColor = ConsoleColor.Yellow;
               Console.WriteLine ($"{except.GetType ().Name}: {except.Message}");
               Console.WriteLine (string.Join ('\n', (except.StackTrace ?? "").Split ('\n').Take (4)));
            }
            ResetColor ();
         }
      }
      BackgroundColor = ConsoleColor.DarkBlue;
      var time = (DateTime.Now - start).TotalSeconds;
      Console.WriteLine ($"{cTests} tests, {cFailed} failed, {cCrash} crashed, {time:F2} seconds.".PadRight (dx));
      ResetColor ();
   }
}