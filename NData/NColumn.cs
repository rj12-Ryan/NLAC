using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLAC
{
    //One column of an NTable, has three data layers (input values:doubles, normal values: ints, lookup values:strings)
    public partial class NColumn
    {
        public NColumn()
        {
            this.InputValues = new List<Double>();
            this.NormalValues = new List<int>();
            this.LookupValues = new List<string>();
        }

        public enum Type
        {
            Input,
            Normal,
            Lookup
        }

        public List<Double> InputValues { get; set; }
        public List<int> NormalValues { get; set; }
        public List<String> LookupValues { get; set; }

        public string Name { get; set; }

        public OptionStager.Instruments Instrument;


        public int Size()
        {
            return this.InputValues.Count();
        }

        //Prints a description of the NColumn (note: we do not override ToString as the length and multi-line strucuture outputed here doesn't fit best practice for ToString)
        public string PrintText()
        {
            string inputSet = (this.InputValues.Count() > 0) ? "Defined" : "Undefined";
            string normalSet = (this.NormalValues.Count() > 0) ? "Defined" : "Undefined";
            string lookupSet = (this.LookupValues.Count() > 0) ? "Defined" : "Undefined";

            string str = string.Empty;

            str += $"NColumn: {this.Name}\n";
            str += $"\tInstrument: {this.Instrument}\n";
            str += "\tData:\n";
            str += $"\t\tInput Values: {inputSet}, {this.InputValues.Count} values\n";
            str += $"\t\tNormal Values: {inputSet}, {this.InputValues.Count} values\n";
            str += $"\t\tLookup Values: {inputSet}, {this.InputValues.Count} values\n";
            
            return str;
        }
    }
}
