/******************************************************************************

Welcome to GDB Online.
GDB online is an online compiler and debugger tool for C, C++, Python, Java, PHP, Ruby, Perl,
C#, OCaml, VB, Swift, Pascal, Fortran, Haskell, Objective-C, Assembly, HTML, CSS, JS, SQLite, Prolog.
Code, Compile, Run and Debug online from anywhere in world.

*******************************************************************************/
/*
    Complete the code in Flock.cs. (1 function per group member; 
    bubblesort is a must - someone must choose this).
    Time the run for each function with 
    number of drones varying from 100 to 1000000.
    (run each multiple times - unless each run takes too long).
    Save result to CSV (google up).
    Plot the runtime for each function (using Excel/Sheet)
    Perform timing test on 3 different machines (with diff spec).
    Present timing test in a report. Include machine spec.
*/

using System;
class HelloWorld {
  static void Main() {
      
      int numRepeat = 100;
      int max = 1000; //1000000;
      int min = 100;
      int stepsize = 100;
      int numsteps = (max-min) / stepsize;
      
      // repeat this declaration for all other functions
      float[] timeAverage = new float[numsteps];
      for (int i=0; i<numsteps; i++)
      {
          int numdrones = i * stepsize + min;
          Console.WriteLine("Current num drones = "+numdrones);
          
          Flock flock = new Flock(numdrones);
          flock.Init((int) (0.9*numdrones)); // fill up 90%
          
          // calculate time for average
          var watch = new System.Diagnostics.Stopwatch();
          watch.Start();
          for (int rep=0; rep<numRepeat; rep++)
          {
              flock.average();
          }
          watch.Stop();   
          long time = watch.ElapsedMilliseconds;
          // store value
          timeAverage[i] = time / numRepeat;

            // ... repeat for all other functions ...
      }
      
      // write results to csv files
      // see https://www.csharptutorial.net/csharp-file/csharp-write-csv-files/
      
  }
}