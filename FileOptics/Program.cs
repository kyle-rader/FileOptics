using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Reflection;

namespace FileOptics
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                int returnCode = BuildApp().Execute(args);
                Environment.Exit(returnCode);
                return returnCode;
            }
            catch (UnrecognizedCommandParsingException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }

        static CommandLineApplication BuildApp()
        {
            var app = new CommandLineApplication<Cli>();
            app.HelpOption("-h|--help");
            app.VersionOption("-v|--version", GetVersion());
            app.Conventions
                .UseConstructorInjection(Services())
                .UseDefaultConventions();
            return app;
        }

        static string GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }

        static IServiceProvider Services()
        {
            return new ServiceCollection()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                .AddTransient<HexWriter>()
                .BuildServiceProvider();
        }
    }
}
