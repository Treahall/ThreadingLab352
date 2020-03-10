using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//
//Author: Trever Hall
//File: Program.cs
//Desctiption: Uses threads to utilize a function that simulates throwing darts to aproximate pi with the Monte Carlo method.
//Purpose: To get introduced to working with threads in c#.
//


namespace ThreadingLab352
{
    class Program
    {
        
        static void Main(string[] args)
        {
            int NumThreads, ThreadDarts, DartsLanded = 0;

            //Get user input for parameters
            Console.Write("How many threads do you want to make: ");
            NumThreads = Convert.ToInt32(Console.ReadLine());
            Console.Write("How many darts for each thread to throw: ");
            ThreadDarts = Convert.ToInt32(Console.ReadLine());

            //Lists used to store the threads and objects in each thread respectively.
            List<Thread> threads = new List<Thread>();
            List<FindPiThread> ThreadObjects = new List<FindPiThread>();

            //Creates and starts a stopwatch before calculations start.
            Stopwatch watch = new Stopwatch();
            watch.Start();

            //Loop to set up threads and start them.
            for(int i = 0; i < NumThreads; i++)
            {
                FindPiThread obj = new FindPiThread(ThreadDarts);
                ThreadObjects.Add(obj);
                Thread thread = new Thread( new ThreadStart(obj.ThrowDarts));
                threads.Add(thread);
                thread.Start();
                //pause for 16 milliseconds so every random has a unique seed.
                Thread.Sleep(16); 
            }

            //Makes sure it waits until every thread is done.
            foreach (Thread item in threads)
            {
                item.Join();
            }

            //Adds up total number of dartsLanded across all threads that ran.
            foreach (FindPiThread item in ThreadObjects)
            {
                DartsLanded = DartsLanded + item.getDartsOnBoard();
            }

            //calculates Pi estimate based on results, then stop the watch.
            double PiEstimate = 4 * ((double)DartsLanded / ((double)ThreadDarts * (double)NumThreads));
            watch.Stop();

            Console.WriteLine($"The estimation of pi based on the results is: {PiEstimate}.");
            Console.WriteLine($"The elapsed time for calculation was {(double)watch.ElapsedMilliseconds / 1000} seconds.");
            Console.Read();
        }

        
    }

    //hold thread state and house the thread function
    class FindPiThread
    {
        int DartsToThrow;
        int DartsOnBoard;
        Random rand;

        public FindPiThread(int NumDarts)
        {
            DartsToThrow = NumDarts;
            DartsOnBoard = 0;
            rand = new Random();
        }

        //Randomly generates the x and y coordinates (between 0.0 and 1.0).
        //Then uses pythagorean theorem to see if its less then 1 (aka inside the unit circle).
        //If it is then DartsOnBoard is updated accordingly.
        public void ThrowDarts()
        {
            double x, y, Pythag;
            for(int i = 0; i < DartsToThrow; i++)
            {
                x = rand.NextDouble();
                y = rand.NextDouble();
                Pythag = Math.Sqrt((x * x) + (y * y));

                if (Pythag <= 1)
                {
                    DartsOnBoard = DartsOnBoard + 1;
                }
            }
               
            
        }
        //Accessor for DartsOnBoard.
        public int getDartsOnBoard()
        {
            return DartsOnBoard;
        }
    }

}
