using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace FileOptics
{
    class HexWriter
    {
        private readonly IConsole console;
        private readonly IFileSystem fs;
        private int sepSpace = 2;
        private string sep;
        private bool withReadableLine;
        private int lineNumber = 0;
        private string lineNumberHeader = string.Empty;
        private readonly StringBuilder sb = new StringBuilder();
        private int value;
        private readonly IList<int> values = new List<int>();

        const int LineFeed = 0x0a;
        const int CarriageReturn = 0x0d;
        const string NonPrintable = "NP";

        IDictionary<int, string> printAs = new Dictionary<int, string>()
        {
            {LineFeed, "\\n" },
            {CarriageReturn, "\\r" },
        };

        public HexWriter(IConsole console, IFileSystem fs)
        {
            this.console = console;
            this.fs = fs;
        }

        public void Write(string file, bool withReadableLine, int sepSpace = 2, bool printBom = true)
        {
            this.sepSpace = sepSpace;
            this.sep = new string(' ', sepSpace);
            this.withReadableLine = withReadableLine;

            console.WriteLine($"File: {file}\n");

            using (var reader = fs.File.OpenRead(file))
            {
                CheckBom(reader, printBom);
                while ((value = reader.ReadByte()) != -1)
                {
                    if (values.Count == 0)
                    {
                        lineNumberHeader = string.Format("{0,-4:d}: ", lineNumber++);
                        sb.Append(lineNumberHeader);
                    }

                    values.Add(value);
                    sb.AppendFormat("{0,2:X2}", value);

                    if (value == LineFeed)
                    {
                        PrintLine();
                    }
                    else
                    {
                        sb.Append(sep);
                    }
                }

                if (values.Count > 0)
                {
                    PrintLine();
                }
            }
        }

        private void CheckBom(Stream reader, bool print)
        {
            int[] bom = new int[3];
            for (var i = 0; i < 3; i++)
            {
                bom[i] = reader.ReadByte();
            }
            string message = string.Empty;

            if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
            {
                message = string.Format("{0:X2} {1:X2} {2:X2} (UTF-8 BOM)", bom[0], bom[1], bom[2]);
            }
            else if (bom[0] == 0xFE && bom[1] == 0xFF)
            {
                message = string.Format("{0:X2} {1:X2} (UTF-16 Big Endian BOM)", bom[0], bom[1]);
                reader.Seek(-1, SeekOrigin.Current); // Seek back 1 byte
            }
            else if (bom[0] == 0xFF && bom[1] == 0xFE)
            {
                message = string.Format("{0:X2} {1:X2} (UTF-16 Little Endian BOM)", bom[0], bom[1]);
                reader.Seek(-1, SeekOrigin.Current); // Seek back 1 byte
            }
            else
            {
                // Reset at start of stream.
                message = "No visible BOM";
                reader.Seek(0, SeekOrigin.Begin);
            }
            if (print)
            {
                console.WriteLine(message);
            }
        }

        void PrintLine()
        {
            console.WriteLine(sb.ToString());
            sb.Clear();

            if (withReadableLine)
            {
                console.WriteLine(ReadableLine(values, lineNumberHeader.Length));
            }

            values.Clear();
        }

        string ReadableLine(IList<int> values, int pad)
        {
            var sb = new StringBuilder();
            sb.Append(new string(' ', pad));
            foreach (var val in values)
            {
                sb.AppendFormat("{0,2:s}{1:s}", ByteAsString(val), sep);
            }
            return sb.ToString();
        }

        string ByteAsString(int value)
        {
            if (printAs.ContainsKey(value)) { return printAs[value]; }

            char c = (char)value;
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
            return NonPrintable;
        }

    }
}
