using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLAC
{
    //Lookup instructions for an NTable
    public partial class NTable
    {
        public void Lookup(LUT lut) 
        { 
            
            _notes = lut.GetNotes();
            Parallel.ForEach(this.Columns, c =>
            {
                _threadedLookup(c);
            });

            //get OptionStager instance from the singleton
            OptionStager o = OptionStager.GetInstance();
            //if in verbose output table
            if (o.Verbose)
            {
                Output.NTable(this, NColumn.Type.Lookup, "Looked-up Data");
            }
        }
        private void _threadedLookup(NColumn c)
        {
			string[] output = new string[c.Size()];
			for (int i = 0; i < c.Size(); i++)
			{
				
				if (c.NormalValues[i] < 0)
				{
                    //add rest for negative normal values (placeholder for double.NaNs after normalization)
                    c.LookupValues.Add("z");
				}
				else
				{
					c.LookupValues.Add(_notes[c.NormalValues[i]]);
				}
			}
		}

        private List<string> _notes;
    }
}
