using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLAC
{
    //Program command line arguments
    //TODO: add flag for JSON parsing mode (-j?)
    public class CLArgs
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool verbose { get; set; }

        [Option('r', "read", Required = true, HelpText = "Input files to be processed.")]
        public string? inputFile { get; set; }

        [Option('c', "cuda", Required = false, HelpText = "Will perform normalization on available CUDA cores (slower for most datasets)")]
        public bool cuda { get; set; }

        [Option('x', "xml", Required = false, HelpText = "Outputs a MusicXML File")]
        public bool xml { get; set; }
    }
}
