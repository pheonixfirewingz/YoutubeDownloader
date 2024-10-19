using CommandLine;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloaderCLI
{
    interface IOptions
    {
        [Option('f', "format",
            HelpText = "should video or mp3",
            Required = true)]
        string Format { get; set; }

        [Value(0, MetaName = "link",
            HelpText = "Input link to be processed.",
            Required = true)]
        string Link { get; set; }

        [Value(1, MetaName = "output",
        HelpText = "Input file to be processed.",
        Required = true)]
        string Output { get; set; }
    }

    [Verb("ProgramOptions", HelpText = "YoutubeDownloaderCLI command help")]
    class Options : IOptions
    {
        public required string Format { get; set; }
        public required string Link { get; set; }
        public required string Output { get; set; }
    }
}
