using CommandLine;
using NLAC;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NLAC
{
    class Program
    {
        static void Main(string[] args)
        {
            //instantiate a new NTiming
            NTiming timing = new NTiming();

            //get a new OptionStager from the singleton
            OptionStager o = OptionStager.GetInstance();

            //parse command line to the OptionStager
            CommandLine.Parser.Default.ParseArguments<CLArgs>(args)
           .WithParsed(RunOptions)
           .WithNotParsed(HandleParseError);
            void RunOptions(CLArgs cl)
            {
                o.setFileName(cl.inputFile);
                o.Verbose = cl.verbose;
                o.CUDA = cl.cuda;
                o.XML = cl.xml;
                
            }
            void HandleParseError(IEnumerable<Error> errs)
            {
                Environment.Exit(160);
            }

            //instantiate a new importer
            Importer importer = new Importer(o.fileName);

            //import into a new NTable
            o.Table = importer.Import();

            timing.StartTiming();

            //TEMP//Set some other options as needed
            o.Title = "Test Song Title";
            o.Composer = "NLAC";
            foreach (NColumn col in o.Table.Columns)
            {
                col.Instrument = OptionStager.Instruments.AcousticGrandPiano;
            }

            //normalize the NTable
            LUT lut = new LUT(LUT.Keys.A, LUT.Modes.Major);
            o.Table.Normalize(lut);


            //lookup the normalized NTable
            o.Table.Lookup(lut);


            //build ABC
            string output = "";
            output += ABCBuilder.Header(lut);
            output += ABCBuilder.Body(o.Table) + "| \\\r\n";

            timing.StopTiming();

            Output.Standard(output);

            //Debug output for verbose mode
            if (o.Verbose)
            {
                Output.NTableDebug(o.Table);
            }

            timing.ResultsOut();


            

            //if xml mode, gen xml
            if (o.XML)
            {
                //read test
                //NMXML.parseXML("meridenWeather.xml");


                //set temp column names because that isn't implemented in nlac yet whoops
                int i = 0;
                foreach (NColumn col in o.Table.Columns)
                {
                    col.Name = $"Column {i} Name";
                    i++;
                }
                NMXML.genXML(o);
            }


        }
    }
            
}

