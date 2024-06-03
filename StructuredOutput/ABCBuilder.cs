using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLAC
{
    //Builds a final output string containing all the ABC data from a fully processed NTable with valid lookup values
    public static class ABCBuilder
    {
        public static string Header(LUT lut)
        {
            //get the OptionStager from the singleton
            OptionStager o = OptionStager.GetInstance();

            string output = "";
            string newLine = "\r\n";

            output = "X: 1" + newLine;
            output += "T: " + o.Title + newLine;
            output += "C: " + o.Composer + newLine;
            output += "M: " + o.Meter + newLine;
            output += "L: 1/4" + newLine;
            output += "Q: " + o.Meter + "=" + o.Tempo.ToString("#.000") + newLine;
            output += "K: " + lut.KeyName + newLine;

            return output;
        }

        public static string Body(NTable t)
        {
            string output = "";

            string[] threadReturn = new string[t.Columns.Count]; 

            //multithreaded iteration through each column
            Parallel.ForEach(t.Columns, c =>
            {
                int index = t.Columns.IndexOf(c);
                threadReturn[index] = _threadedGenerator(c, index);
            });

            //rebuild the output from each thread

            List<string> results = new List<string>();
            foreach(string threadResult in threadReturn)
            {
                results.Add(threadResult);
            }
            return string.Join("\r\n",threadReturn);
        }

        //generate voice header and note data from a column
        private static string _threadedGenerator(NColumn c, int index)
        {
            string output = "";
            string newLine = "\r\n";
            string adjustedIndex = $"{(index + 1)}";
            output += "V: " + adjustedIndex + $" name=\"{c.Name}\"" + newLine;
            output += "%%MIDI program " + (int)c.Instrument + newLine;

            output += string.Join("| \\\r\n", c.LookupValues);

            return output;
        }
    }
}
