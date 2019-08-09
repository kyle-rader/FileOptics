using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Abstractions;
using System.Linq;

namespace FileOptics
{
    [Command(
        Name = "fo",
        FullName = "FileOptics",
        Description = "A file inspector application")]
    class Cli
    {
        private IConsole console;
        private IFileSystem fs;

        [Argument(0,
            Name = "FILE",
            Description = "The relative or absolute path to the file you want optics into.")]
        [Required]
        [FileExists]
        string File { get; set; }

        public Cli(IConsole console, IFileSystem fs)
        {
            this.console = console;
            this.fs = fs;
        }

        IDictionary<int, string> printAs = new Dictionary<int, string>()
        {
            {0x0a, "\\n" },
            {0x0d, "\\r" },
        };

        int OnExecute()
        {
            console.WriteLine($"File: {File}\n");
            using (var reader = fs.File.OpenRead(File))
            {
                int datum;
                while ((datum = reader.ReadByte()) != -1)
                {
                    console.Write("{0:X2} ({1,-3:s}", datum, StringDatum(datum) + ")");
                    if (datum == 0x0a)
                    {
                        console.Write("\n");
                    }
                    else
                    {
                        console.Write(" ");
                    }
                }
                console.Write("\n");
            }
            return 0;
        }

        string StringDatum(int datum)
        {
            if (printAs.ContainsKey(datum)) { return printAs[datum]; }

            char c = (char)datum;
            bool normal = new[]
            {
                char.IsLetterOrDigit(c),
                char.IsPunctuation(c),
                char.IsWhiteSpace(c),
            }.Any();

            if (normal)
            {
                return c.ToString();
            }
            else if (char.IsSeparator(c))
            {
                return char.GetUnicodeCategory(c).ToString();
            }
            return datum.ToString("X2");
        }
    }
}
