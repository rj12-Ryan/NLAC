using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;


namespace NLAC
{
    //Wrapper for System.Threading stopwatch with specific formatted output 
    internal class NTiming
    {
        public NTiming() 
        { 
            _stopwatch = new Stopwatch();
        }
        private Stopwatch _stopwatch;

        public void StartTiming()
        {
            _stopwatch.Start();
        }

        public void StopTiming()
        {
            _stopwatch.Stop();
        }

        public void ResultsOut()
        {
            //get OptionStager from singleton
            OptionStager o = OptionStager.GetInstance();

            float milliseconds = _stopwatch.ElapsedMilliseconds;
            string elapsedTimeText = "";

            //format the time string
            if (milliseconds <= 1000)
            {
                elapsedTimeText = " " + milliseconds.ToString() + " milliseconds";
            }
            else if (milliseconds < 1)
            {
                elapsedTimeText = " less than 1 millisecond";
            }
            else if (milliseconds > 1000)
            {
                elapsedTimeText = " " + (milliseconds / 1000).ToString("F2") + " seconds";
            }

            //calculate and display notes per second
            float nps =  o.NoteCount / (milliseconds / 1000);
            string snps = " (" + nps.ToString("N2") + " notes/second)";

            Output.Standard("Generated " + o.NoteCount.ToString("N0") + " Notes in" + elapsedTimeText + snps);
        }
    }
}
