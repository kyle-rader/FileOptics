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
        static void Main(string[] args)
        {
            var app = new CommandLineApplication<Cli>(throwOnUnexpectedArg: false);
            app.Conventions.UseDefaultConventions();
            app.HelpOption("-h|--help");
            app.VersionOption("-v|--version", GetVersion());

            app.Conventions.UseConstructorInjection(Services());
            Environment.Exit(app.Execute(args));
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
                .BuildServiceProvider();
        }
    }
}
