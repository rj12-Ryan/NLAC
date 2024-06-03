using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NLAC
{
    //A main hypervisor-esque data structure class that essentially stores all information about the current processing task
    public partial class OptionStager
    {
        private OptionStager() { }

        //singleton
        private static OptionStager _instance;
        public static OptionStager GetInstance()
        {
            // If the instance is null, create a new instance
            if (_instance == null)
            {
                _instance = new OptionStager();
            }
            return _instance;
        }

        public int NoteCount;
        public string fileName = "";
        public bool Verbose { get; set; }
        public bool CUDA {  get; set; }
        public bool XML { get; set; }
        public string? Title;
        public string? Composer;
        public string Meter = "1/4";
        public decimal Tempo;


        //store an instance of an NTable in the OptionStager for convience 
        public NTable Table { get; set; }

        public void setFileName(string? f)
        {
            try
            {
                if (f != null && File.Exists(f)) this.fileName = f;
                else throw new FileNotFoundException();
            }
            catch 
            { 
                Output.Error("File Not Found");
                Environment.Exit(2); 
            }
        }
    }
}
