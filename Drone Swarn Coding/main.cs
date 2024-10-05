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
using System.IO;

class HelloWorld
{
    static void Main()
    {
        int numRepeat = 1000;
        int max = 1000; //1000000;
        int min = 100;
        int stepsize = 100;
        int numsteps = (max - min) / stepsize;
        
        float[] timeAverage = new float[numsteps];
        float[] timeMin = new float[numsteps];
        float[] timeMax = new float[numsteps];

        // Name : Nur Fatihah binti Mohd Noor
        // Matric ID : 24000227
        string AverageMaxMinFilePath = "AverageMaxMin_results.csv";
        using (StreamWriter writer = new StreamWriter(AverageMaxMinFilePath)) 
        {
            writer.WriteLine("Number of Drones, Average Time (ms), Max Time (ms), Min Time (ms)");

            for (int i = 0; i < numsteps; i++) 
            {
                int numdrones = i * stepsize + min;
                Console.WriteLine("Current num drones = " + numdrones);
              
                Flock flock = new Flock(numdrones);
                flock.Init((int) (0.9 * numdrones)); // fill up 90%

                var watch = new System.Diagnostics.Stopwatch();

                // Timer for average()
                watch.Start();
                for (int rep = 0; rep < numRepeat; rep++) 
                {
                    flock.average();
                }
                watch.Stop();
                timeAverage[i] = watch.ElapsedMilliseconds / (float)numRepeat;

                // Timer for max()
                watch.Restart();
                for (int rep = 0; rep < numRepeat; rep++)
                {
                    flock.max();
                }
                watch.Stop();
                timeMax[i] = watch.ElapsedMilliseconds / (float)numRepeat;

                // Timer for min()
                watch.Restart();
                for (int rep = 0; rep < numRepeat; rep++) {
                    flock.min();
                }
                watch.Stop();
                timeMin[i] = watch.ElapsedMilliseconds / (float)numRepeat;

                // Write results to CSV
                writer.WriteLine($"{numdrones}, {timeAverage[i]}, {timeMax[i]}, {timeMin[i]}");
            }
        }
        Console.WriteLine($"Results saved to {AverageMaxMinFilePath}");

        // Arrays to store the average execution time for appendFront() and insert() methods
        //Name :Dania Adriana Binti Mohd Faizal 
        // Matric ID: 22006373
        float[] timeAppendFront = new float[numsteps];
        float[] timeInsert = new float[numsteps];

        // Create separate CSV files for appendFront() and insert()
        string appendFrontFilePath = "appendFront_timing_results.csv";
        string insertFilePath = "insert_timing_results.csv";

        // Write headers for appendFront CSV
        using (var writer = new StreamWriter(appendFrontFilePath))
        {
            writer.WriteLine("NumDrones,AppendFrontTime(ms)");  // CSV header for appendFront
        }

        // Write headers for insert CSV
        using (var writer = new StreamWriter(insertFilePath))
        {
            writer.WriteLine("NumDrones,InsertTime(ms)");  // CSV header for insert
        }

        for (int i = 0; i < numsteps; i++)
        {
            int numdrones = i * stepsize + min;
            Console.WriteLine("Current num drones = " + numdrones);

            Flock flock = new Flock(numdrones);
            flock.Init((int)(0.9 * numdrones));  // Initialize the flock with 90% of the drones filled

            // Timing for appendFront()
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            for (int rep = 0; rep < numRepeat; rep++)
            {
                flock.appendFront(new Drone(rep));  // Append new drone at the front
            }
            watch.Stop();
            long appendFrontTime = watch.ElapsedMilliseconds;

            // Store the average time for appendFront
            timeAppendFront[i] = appendFrontTime / (float)numRepeat;

            // Write the results for appendFront to the CSV file
            using (var writer = new StreamWriter(appendFrontFilePath, append: true))
            {
                writer.WriteLine($"{numdrones},{timeAppendFront[i]}");
            }

            // Print the average time for appendFront
            Console.WriteLine($"Average time for appendFront with {numdrones} drones: {timeAppendFront[i]} ms");

            // Timing for insert() at index 1
            watch.Reset();
            watch.Start();
            for (int rep = 0; rep < numRepeat; rep++)
            {
                flock.insert(new Drone(rep), 1);  // Insert new drone at index 1
            }
            watch.Stop();
            long insertTime = watch.ElapsedMilliseconds;

            // Store the average time for insert
            timeInsert[i] = insertTime / (float)numRepeat;

            // Write the results for insert to the CSV file
            using (var writer = new StreamWriter(insertFilePath, append: true))
            {
                writer.WriteLine($"{numdrones},{timeInsert[i]}");
            }

            // Print the average time for insert
            Console.WriteLine($"Average time for insert with {numdrones} drones: {timeInsert[i]} ms");
        }

        Console.WriteLine($"Results saved to {appendFrontFilePath} and {insertFilePath}");

        //Name : Zulaikha binti Mohd Azhar
        // Matric ID : 24000918
        // Insertion sort timing
        float[] timeInsertSort = new float[numsteps];
      
        string csvFilePath = "insertionsort_results.csv";
        using (StreamWriter insertionSortWriter = new StreamWriter(csvFilePath))
        {
            insertionSortWriter.WriteLine("Number of Drones, Insertion Sort Time Average (ms)");
          
            for (int i = 0; i < numsteps; i++)
            {
                int numdrones = i * stepsize + min;
                Console.WriteLine("Current num drones = " + numdrones);
                
                Flock flock = new Flock(numdrones);
                flock.Init((int)(0.9 * numdrones)); // fill up 90%
                
                // calculate time for average
                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                for (int rep = 0; rep < numRepeat; rep++)
                {
                    flock.insertionsort();
                }
                stopwatch.Stop();   
                long time = stopwatch.ElapsedMilliseconds;
                
                // store value
                timeInsertSort[i] = (float)time / numRepeat;
                
                // Write the number of drones and the average time to CSV
                insertionSortWriter.WriteLine($"{numdrones}, {timeInsertSort[i]}");
            }
            Console.WriteLine("Insertion sort results written to " + csvFilePath);
                
            // Bubble Sort Timing
            // Name : ⁠Sara Eudora Binti Said
            // Matric ID: 24000574
            string csvFilePathBubbleSort = "bubblesort_results.csv";
            float[] timeBubbleSort = new float[numsteps]; // Array to store timing results

            using (StreamWriter bubbleSortWriter = new StreamWriter(csvFilePathBubbleSort))
            {
                bubbleSortWriter.WriteLine("Number of Drones, Bubble Sort Time Average (ms)");

                for (int i = 0; i < numsteps; i++) 
                {
                    int numdrones = i * stepsize + min;
                    Console.WriteLine("Current num drones for bubble sort = " + numdrones);

                    Flock flock = new Flock(numdrones);
                    flock.Init((int)(0.9 * numdrones)); // Fill up 90%

                    // Calculate time for bubble sort
                    var stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();
                    for (int rep = 0; rep < numRepeat; rep++) {
                        flock.bubblesort();
                    }
                    stopwatch.Stop();

                    long time = stopwatch.ElapsedMilliseconds;
                    timeBubbleSort[i] = (float)time / numRepeat; // Store average time
                    bubbleSortWriter.WriteLine($"{numdrones}, {timeBubbleSort[i]}"); // Write to CSV
                }
                Console.WriteLine("Bubble sort results written to " + csvFilePathBubbleSort);
            }
            
            // Timing for deleteFront()
            // Name: Nurul Haniizati binti Hazli
            //Matric ID: 24000182
            float[] timeDeleteFront = new float[numsteps];

            // Create CSV files for deleteFront()
            string deleteFrontFilePath = "deleteFront_timing_results.csv";
            

            // Write headers for deleteFront CSV
            using (var deleteFrontWriter = new StreamWriter(deleteFrontFilePath))
            {
                deleteFrontWriter.WriteLine("NumDrones,DeleteFrontTime(ms)");  // CSV header for deleteFront
            }

            for (int i = 0; i < numsteps; i++)
            {
                int numdrones = i * stepsize + min;
                Console.WriteLine("Current num drones = " + numdrones);

                // Timing for deleteFront()
                Flock flock = new Flock(numdrones);
                flock.Init((int)(0.9 * numdrones));  // Initialize the flock with 90% of the drones filled

                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                for (int rep = 0; rep < numRepeat; rep++)
                {
                    flock.deleteFront(0); // Delete the first element (index 0)
                }
                stopwatch.Stop();
                long deleteFrontTime = stopwatch.ElapsedMilliseconds;

                // Store the average time for deleteFront
                timeDeleteFront[i] = deleteFrontTime / (float)numRepeat;

                // Write the results for deleteFront to the CSV file
                using (var deleteFrontWriter = new StreamWriter(deleteFrontFilePath, append: true))
                {
                    deleteFrontWriter.WriteLine($"{numdrones},{timeDeleteFront[i]}");
                }
                Console.WriteLine($"Results saved to {deleteFrontFilePath}");
            }
        }
    }
}
