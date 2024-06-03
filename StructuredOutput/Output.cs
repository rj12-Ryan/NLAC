using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLAC
{
    //Wrappers for standard output and some specific output types for NColumns 
    internal static class Output
    {
        public static void Standard(string message)
        {
            Console.Write(message+"\n");
        }
        
        public static void Error(string message)
        {
            Console.Write("ERROR: " + message + "\n");
        }

        public static void NTable(NTable t, NColumn.Type Type, string ntTitle = "")
        {
            if (ntTitle != "")
            {
                Console.Write(ntTitle + ":\r\n");
            }

            DataTable dt = new DataTable();
            

            switch (Type)
            {
                case NColumn.Type.Input:
                    foreach (NColumn c in t.Columns)
                    {
                        int colIndex = t.Columns.IndexOf(c);
                        dt.Columns.Add("Column " + colIndex, typeof(double));
                    }
                        List<double> irow = new List<double>();

                        for (int i = 0; i < t.Size(); i++)
                        {
                            irow = t.InputRow(i);
                            DataRow dataRow = dt.NewRow();

                            for (int j = 0; j < irow.Count; j++)
                            {
                                dataRow[j] = irow[j];
                            }

                            dt.Rows.Add(dataRow);
                        }
                    
                break;
                case NColumn.Type.Normal:
                    foreach (NColumn c in t.Columns)
                    {
                        int colIndex = t.Columns.IndexOf(c);
                        dt.Columns.Add("Column " + colIndex, typeof(int));
                    }
                    List<int> nrow = new List<int>();

                    for (int i = 0; i < t.Columns[0].InputValues.Count; i++)
                    {
                        nrow = t.NormalRow(i);
                        DataRow dataRow = dt.NewRow();

                        for (int j = 0; j < nrow.Count; j++)
                        {
                            dataRow[j] = nrow[j];
                        }

                        dt.Rows.Add(dataRow);
                    }

                break;
                case NColumn.Type.Lookup:
                    foreach (NColumn c in t.Columns)
                    {
                        int colIndex = t.Columns.IndexOf(c);
                        dt.Columns.Add("Column " + colIndex, typeof(string));
                    }
                    List<string> lrow = new List<string>();

                    for (int i = 0; i < t.Size(); i++)
                    {
                        lrow = t.LookupRow(i);
                        DataRow dataRow = dt.NewRow();

                        for (int j = 0; j < lrow.Count; j++)
                        {
                            dataRow[j] = lrow[j];
                        }

                        dt.Rows.Add(dataRow);
                    }
                break;
            }
            dt.Print();
        }

        public static void NTableDebug(NTable nt)
        {
            //NTable info
            Output.Standard("\nDEBUG:\nNTable Configuration: " + nt.ToString());

            //Columns for NTable
            foreach (NColumn col in nt.Columns)
            {
                Output.Standard($"[{nt.Columns.IndexOf(col)}] " + col.PrintText());
            }
        }
    }
}
