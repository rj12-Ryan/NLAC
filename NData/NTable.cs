using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace NLAC
{
    //A collection of NColumns with addtional features that form the full data structure of an NLAC dataset.
    public partial class NTable
    {
        public NTable() 
        {
            this.Columns = new List<NColumn>();
        }

        public List<NColumn> Columns { get; set; }

        public int Size()
        {
            if(this.Columns.Count > 0)
            {
                //assume table is size of largest column? (seems to work, yippeee!)
                int maxLength = 0;
                foreach (NColumn col in this.Columns)
                {
                    if (col.Size() > maxLength) maxLength = col.Size();
                }
                return maxLength;
            }
            else
            {
                throw new Exception("Cannot return size of an empty NTable");
            }
        }

        public List<double> InputRow(int r) 
        {
            if(this.Columns.Count > 0)
            {
                List<double> result = new List<double>();
                foreach(NColumn col in this.Columns)
                {
                    result.Add(col.InputValues[r]);
                }
                return result;
            }
            else
            {
                throw new Exception("Cannot return row from an empty NTable");
            }
        }

        public List<int> NormalRow(int r)
        {
            if (this.Columns.Count > 0)
            {
                List<int> result = new List<int>();
                foreach (NColumn col in this.Columns)
                {
                   
                    result.Add(col.NormalValues[r]);
 
                }
                return result;
            }
            else
            {
                throw new Exception("Cannot return row from an empty NTable");
            }
        }

        public List<string> LookupRow(int r)
        {
            if (this.Columns.Count > 0)
            {
                List<string> result = new List<string>();
                foreach (NColumn col in this.Columns)
                {
                    try
                    {
                        result.Add(col.LookupValues[r]);
                    }
                    catch
                    {
                        throw new Exception("Could not retrieve lookup values, likely that lookup isn't complete");
                    }

                }
                return result;
            }
            else
            {
                throw new Exception("Cannot return row from an empty NTable");
            }
        }

        public override string ToString()
        {
            return $"NTable of size {this.Size()} containing {this.Columns.Count} NColumns"; ;
        }

    }
}
