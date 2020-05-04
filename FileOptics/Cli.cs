using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FileOptics
{
    [Command(
        Name = "fo",
        FullName = "FileOptics",
        Description = "A file inspector application")]
    class Cli
    {
        [Argument(0,
            Name = "FILE",
            Description = "The relative or absolute path to the file you want optics into.\n")]
        [Required]
        [FileExists]
        string File { get; set; }

        [Option("-r|--readable-lines",
            CommandOptionType.NoValue,
            Description = "Print readable lines under hex values.\n                       Default: false\n")]
        bool ReadableLines { get; set; } = false;

        [Option("-s|--sep <SEP>",
            CommandOptionType.SingleValue,
            Description = "The amount of spaces to separator each hex value by.\n                       Default: 1\n")]
        int SeparatorLength { get; set; } = 1;

        [Option("--no-bom",
            CommandOptionType.NoValue,
            Description = "Skip printing BOM (byte order mark) info.")]
        bool SkipBom { get; set; } = false;

        int OnExecute(HexWriter hexWriter, IConsole console)
        {
            try
            {
                hexWriter.Write(File, ReadableLines, SeparatorLength, printBom: !SkipBom);
            }
            catch (Exception ex)
            {
                console.Error.WriteLine(ex.Message);
                console.Error.WriteLine(ex.StackTrace);
                return 1;
            }
            return 0;
        }
    }
}
