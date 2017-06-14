using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Builder
{
    public static partial class Program
    {
        private static bool Quiet { get; set; }

        private static void Main(string[] args)
        {
            bool failed = false;
            bool clean = false;
            bool quiet = false;

            if (args.Length >= 2)
            {
                var strInput = args[0];
                var strOutput = args[1];
                for (int i = 2; i < args.Length && !failed; i++)
                {
                    switch (args[i])
                    {
                        case "-q":
                        case "--quiet":
                            quiet = true;
                            break;
                        case "-c":
                        case "--clean":
                            quiet = true;
                            break;
                        default:
                            failed = true;
                            break;
                    }
                }
                if (!failed)
                {
                    Quiet = quiet;
                    Log.OnLog += Log_OnLog;
                    try
                    {
                        if (!File.Exists(strInput))
                            Log.Error($"Unable to find {strInput} project.");
                        if (!Directory.Exists(strOutput))
                            Directory.CreateDirectory(strOutput);
                        var project = Project.Open(strInput);
                        if (clean) Clean(project, strOutput);
                        else Build(project, strOutput);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Unhandled exception {e.GetType()} in {e.Source}\nInfo: {e.Message}\nStack trace:\n{e.StackTrace}");
                    }
                }
            }
            else
                failed = true;
            if (failed)
                ShowUsage();
        }

        private static void Log_OnLog(Log.Level level, string message)
        {
            ConsoleColor color;
            switch (level)
            {
                case Log.Level.Error:
                    color = ConsoleColor.Red;
                    break;
                case Log.Level.Warning:
                    if (Quiet) return;
                    color = ConsoleColor.Yellow;
                    break;
                case Log.Level.Message:
                default:
                    if (Quiet) return;
                    color = ConsoleColor.Gray;
                    break;
            }
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }

        private static void ShowInfo()
        {
            Log.Message("XeBuilder - XeEngine\nDeveloped by Luciano (Xeeynamo) Ciccariello.\n");
        }
        private static void ShowUsage()
        {
            ShowInfo();
            Console.WriteLine("xebuilder <project.proj.json> <output dir> [-c|--clean] [-q|--quiet]\n");
        }
    }
}
