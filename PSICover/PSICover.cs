namespace PSICover;
using System.Diagnostics;
using System.Text.RegularExpressions;

// The CoverageAnalyzer for .Net
class Analyzer {
   public Analyzer (string dir, string runExe, params string[] modules) {
      Dir = dir; RunExe = runExe; Modules = modules.ToList ();
   }
   readonly string Dir;
   readonly string RunExe;
   readonly List<string> Modules;

   public void Run () {
      Console.WriteLine ("Make backups");
      Modules.ForEach (MakeBackup);
      try {
         Modules.ForEach (Disassemble);
         Modules.ForEach (AddInstrumentation);
         Modules.ForEach (Assemble);
         RunCode ();
         GenerateOutputs ();
      } finally {
         Modules.ForEach (RestoreBackup);
      }
   }

   // Make backups of the DLL and PDB files
   void MakeBackup (string m) {
      Directory.CreateDirectory ($"{Dir}/Backups");
      File.Copy ($"{Dir}/{m}", $"{Dir}/Backups/{m}", true);
      var pdb = Path.ChangeExtension (m, ".pdb");
      File.Copy ($"{Dir}/{pdb}", $"{Dir}/Backups/{pdb}", true);
   }

   // Disassemble the DLL to create IL assembly files
   void Disassemble (string module) {
      Console.WriteLine ($"Disassembling {module}");
      var ildasmNew = $"{Dir}/ASMCore/ildasm.exe";
      var ildasmOld = $"{Dir}/ASMFramework/ildasm.exe";
      ExecProgram (ildasmOld, $"/LINENUM /TOKENS /out={Dir}/lines.asm {Dir}/{module}");
      ExecProgram (ildasmNew, $"/TOKENS /out={Dir}/nolines.asm {Dir}/{module}");

      string[] text1 = File.ReadAllLines ($"{Dir}/lines.asm").Where (a => !string.IsNullOrWhiteSpace (a)).ToArray ();
      List<string> text2 = File.ReadAllLines ($"{Dir}/nolines.asm").Where (a => !string.IsNullOrWhiteSpace (a)).ToList ();
      int n2 = 0;
      for (int n1 = 0; n1 < text1.Length; n1++) {
         var line = text1[n1].Trim ();
         if (line.StartsWith (".method /*")) {
            // Sync pointer n2 to the same method in the nolines.asm text
            for (; ; n2++)
               if (text2[n2] == text1[n1]) break;
            n2++;
            continue;
         }
         if (line.StartsWith (".line") && !line.StartsWith (".line 16707566")) {
            var match = rIL.Match (text1[n1 + 1].Trim ());
            if (match.Success) {
               SeekTo (match.Value);
               text2.Insert (n2, text1[n1]); n2++;
               continue;
            }
            match = rIL.Match (text1[n1 - 1].Trim ());
            if (match.Success) {
               SeekTo (match.Value);
               text2.Insert (n2 + 1, text1[n1]); n2++;
               continue;
            }
            throw new Exception ($"Could not match {line}");
         }
      }
      var asmFile = Path.ChangeExtension (module, ".original.asm");
      File.WriteAllLines ($"{Dir}/{asmFile}", text2.ToArray ());
      File.Delete ($"{Dir}/lines.asm");
      File.Delete ($"{Dir}/nolines.asm");

      // Helper .................................
      void SeekTo (string label) {
         for (; ; n2++) {
            var line2 = text2[n2].Trim ();
            if (line2.StartsWith (".method /*")) throw new Exception ("Found next method");
            var match1 = rIL.Match (line2);
            if (match1.Value == label) return;
         }
      }
   }
   static Regex rIL = new (@"^IL_[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]:", RegexOptions.Compiled);

   // Add the instrumentation (add a hit after each .line)
   void AddInstrumentation (string m) {
      Console.WriteLine ($"Instrumenting {m}");
      var infile = $"{Dir}/{Path.GetFileNameWithoutExtension (m)}.original.asm";
      var outfile = $"{Dir}/{Path.GetFileNameWithoutExtension (m)}.asm";
      List<string> src = File.ReadAllLines (infile).Select (ModifyJumps).ToList ();
      List<string> dest = new ();
      for (int i = 0; i < src.Count; i++) {
         string s1 = src[i];
         dest.Add (s1);
         if (s1.Trim ().StartsWith (".line ")) {
            string s2 = src[i + 1];
            int colon = s2.IndexOf (':') + 1;
            if (colon != 0) {
               var match = mRxLine.Match (s1);
               if (!match.Success) throw new NotImplementedException ();
               var groups = match.Groups;
               mBlocks.Add (new Block (mBlock, int.Parse (groups[1].Value), int.Parse (groups[2].Value),
                  int.Parse (groups[3].Value), int.Parse (groups[4].Value), groups[5].Value));

               string label = s2[..colon];
               dest.Add ($"{label}  ldc.i4 {mBlock}");
               dest.Add ($"              call void [CoverLib]CoverLib.HitCounter::Hit(int32)");
               dest.Add (new string (' ', colon) + s2[colon..]);
               mBlock++; i++;
            }
         }
      }
      File.WriteAllLines (outfile, dest);
   }
   static string ModifyJumps (string s) {
      if (!s.Contains (".s ")) return s;
      foreach (var jump in sJumps)
         s = s.Replace ($" {jump}.s ", $" {jump} ");
      return s; 
   }
   static string[] sJumps = new[] {
      "leave", "br", "beq", "bge", "bge.un", "bgt", "bgt.un",
      "ble", "ble.un", "blt", "blt.un", "bne", "bne.un",
      "brfalse", "brnull", "brzero", "brtrue", "brinst" 
   };
   int mBlock = 0;
   static Regex mRxLine = new Regex (@"\.line (\d*),(\d*) : (\d*),(\d*) '(.*)'", RegexOptions.Compiled);
   List<Block> mBlocks = new List<Block> ();

   // Re-assemble instrumented DLLs from the modified ASMs
   void Assemble (string module) {
      File.Delete ($"{Dir}/{module}");
      var ilasm = $"{Dir}/ASMCore/ilasm.exe";
      var asmfile = $"{Dir}/{Path.GetFileNameWithoutExtension (module)}.asm";
      ExecProgram (ilasm, $"/QUIET /dll /PE64 /X64 {asmfile} /output={Dir}/{module}");
   }

   // Run the instrumented program to gather data (hits)
   void RunCode () {
      Console.WriteLine ("Running the code");
      RunProcess ($"{Dir}/{RunExe}", "");
   }

   // Generate output HTML (colored source code with hit / unhit areas marked)
   void GenerateOutputs () {
      Console.WriteLine ("Generating HTML outputs");
      Directory.CreateDirectory ($"{Dir}/HTML");
      ulong[] hits = File.ReadAllLines ($"{Dir}/hits.txt").Select (ulong.Parse).ToArray ();
      var files = mBlocks.Select (a => a.File).Distinct ().ToList ();

      int bTotal = mBlocks.Count, bHit = hits.Take (bTotal).Count (a => a > 0);
      double percent = Math.Round (100.0 * bHit / bTotal, 1);
      Console.WriteLine ($"Coverage: {bHit}/{bTotal}, {percent}%");

      foreach (var file in files) {
         var blocks = mBlocks.Where (a => a.File == file)
                             .OrderBy (a => a.SPosition)
                             .ThenByDescending (a => a.EPosition)
                             .ToList ();
         for (int i = blocks.Count - 1; i > 0; i--)
            if (blocks[i - 1].Contains (blocks[i]))
               blocks.RemoveAt (i - 1);
         blocks.Reverse ();

         var code = File.ReadAllLines (file);
         foreach (var b in blocks) {
            bool covered = hits[b.Id] > 0;
            code[b.ELine] = code[b.ELine].Insert (b.ECol, "~end~");
            code[b.SLine] = code[b.SLine].Insert (b.SCol, covered ? "~green~" : "~red~");
         }
         for (int i = 0; i < code.Length; i++) {
            var line = code[i].Replace ("<", "&lt;").Replace (">", "&gt;");
            line = line.Replace ("~green~", "<span class=\"hit\">");
            line = line.Replace ("~red~", "<span class=\"unhit\">");
            code[i] = line.Replace ("~end~", "</span>");
         }

         string html = $$"""
            <html><head><style>
            .hit { background-color:aqua; }
            .unhit { background-color:orange; }
            </style></head>
            <body><pre>
            {{string.Join ("\r\n", code)}}
            </pre></body></html>
            """;
         File.WriteAllText ($"{Dir}/HTML/{Path.GetFileNameWithoutExtension (file)}.html", html);
      }
   }

   // Restore the DLLs and PDBs from the backups
   void RestoreBackup (string m) {
      Console.WriteLine ("Restoring from backups");
      File.Copy ($"{Dir}/Backups/{m}", $"{Dir}/{m}", true);
      var pdb = Path.ChangeExtension (m, ".pdb");
      File.Copy ($"{Dir}/Backups/{pdb}", $"{Dir}/{pdb}", true);
   }

   // Execute an external program, and wait for it to complete
   // (Also throws an exception if the external program returns a non-zero error code)
   static void ExecProgram (string name, string args) {
      var proc = Process.Start (name, args);
      proc.WaitForExit ();
      if (proc.ExitCode != 0)
         throw new Exception ($"Process {name} returned code {proc.ExitCode}");
   }
}

// Represents a basic code-coverage block (contiguous block of C# code)
class Block {
   public Block (int id, int sLine, int eLine, int sCol, int eCol, string file) {
      if (file == "") file = sLastFile;
      (Id, SLine, ELine, SCol, ECol, File) = (id, sLine - 1, eLine - 1, sCol - 1, eCol - 1, file);
      sLastFile = file;
   }

   public bool Contains (Block c) {
      if (File != c.File) return false;
      if (c.SPosition < SPosition) return false;
      if (c.EPosition > EPosition) return false;
      return true;
   }

   public override string ToString () 
      => $"{SLine},{ELine} : {SCol},{ECol} of {File}";

   public readonly int Id;
   public readonly int SLine, ELine, SCol, ECol;
   public int SPosition => SLine * 10000 + SCol;
   public int EPosition => ELine * 10000 + ECol;
   public readonly string File;
   static string sLastFile = "";
}

static class Program {
   public static void Main () {
      var analyzer = new Analyzer ("P:/Bin", "PSITest.exe", "parser.dll");
      analyzer.Run ();
   }
}
