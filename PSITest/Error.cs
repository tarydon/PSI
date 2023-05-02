namespace PSITest;

[TestFixture (2, "Tests demonstrating various parse errors")]
class ParseErrors {
   [Test (7, "Incorrect number of parameters to a function")]
   void Test1 () => Test ("Error/ArgCount.pas");

   [Test (8, "Invalid type of argument to a function")]
   void Test2 () => Test ("Error/ArgType.pas");

   [Test (9, "Variables must be assigned before use")]
   void Test3 () => Test ("Error/Assign1.pas");

   [Test (10, "Unterminated comment")]
   void Test4 () => Test ("Error/Comment.pas");

   [Test (11, "Const name reused for Variable")]
   void Test5 () => Test ("Error/DupeName.pas");

   [Test (12, "Var name reused for Function")]
   void Test6 () => Test ("Error/DupeName2.pas");

   [Test (13, "Function name must be assigned to")]
   void Test7 () => Test ("Error/FnResult.pas");

   [Test (14, "For loop control variable must be integer")]
   void Test8 () => Test ("Error/ForLoop.pas");

   [Test (15, "If condition must be of boolean type")]
   void Test9 () => Test ("Error/Ifthen.pas");

   [Test (16, "Ordering of const and var")]
   void Test10 () => Test ("Error/Ordering.pas");

   [Test (17, "Repeat statement condition should be bool")]
   void Test11 () => Test ("Error/Repeat.pas");

   [Test (18, "Don't use reserved words as identifiers")]
   void Test12 () => Test ("Error/Reserved.pas");

   [Test (19, "Scoping of variables")]
   void Test13 () => Test ("Error/Scope.pas");

   [Test (20, "While loop condition should be bool")]
   void Test14 () => Test ("Error/While.pas");

   static public void Test (string file) => DemoFiles.Test (file);
}