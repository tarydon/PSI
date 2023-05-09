namespace CoverLib;

public static class HitCounter {
   // A dummy function you can call to add a dependency to this CoverLib library
   public static void Dummy () { }

   // Called to register a particular block is hit
   public static void Hit (int block) {
      while (block >= mHits.Length)
         Array.Resize (ref mHits, mHits.Length * 2);
      mHits[block]++;
   }
   static ulong[] mHits = new ulong[128];

   // Called to save all the hit-counters to a text file (one per line)
   public static void Save (string file) {
      File.WriteAllLines (file, mHits.Select (a => a.ToString ()));
   }
}