using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLAC;
using System.Security.Cryptography.X509Certificates;
using CommandLine;

namespace NLAC
{
    //Class that takes a filepath to a delimited UTF-8 text file and a delimeter (default is comma) and will attempt to parse the data into an NTable structure
    public class Importer
    {
        public Importer(string filePath, string delimter = ",")
        {
            //Configure Parser

            if (File.Exists(filePath))
            {
                _parser = new TextFieldParser(filePath);
            }
            else
            {
                //throw new ArgumentException("File Does Not Exist", "filePath");
                Output.Error("File Read Error");
            }



            _parser.TextFieldType = FieldType.Delimited;
            _parser.SetDelimiters(delimter);
            _parser.HasFieldsEnclosedInQuotes = true;
        }

        private TextFieldParser _parser;
        public NTable Import()
        {
            NTable t = new NTable();

            using (_parser)
            {
                // Read data rows
                while (!_parser.EndOfData)
                {
                    string[] fields = _parser.ReadFields();

                    // Initialize columns if not already initialized
                    if (t.Columns.Count == 0)
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {
                            t.Columns.Add(new NColumn());
                        }
                    }

                    // Populate lists for each column
                    for (int i = 0; i < fields.Length; i++)
                    {
                        
                        double value;
                        if (double.TryParse(fields[i], out value))
                        {
                            //if value can become a double add it
                            t.Columns[i].InputValues.Add(value);
                        }
                        else
                        {
                            //otherwise add a NaN
                            t.Columns[i].InputValues.Add(double.NaN);
                        }
                        
                    }
                }
            }

            //if in verbose print input data
            //get the OptionStager from the singleton
            OptionStager o = OptionStager.GetInstance();
            if (o.Verbose)
            {
                Output.NTable(t, NColumn.Type.Input, "Input Data");
            }

            //record total notes imported
            o.NoteCount = t.Columns.Count * t.Columns[0].InputValues.Count;

            //return the NTable
            return t;
            }
        }

    }



