﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Tools.Projects;

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
                    Log.OnLog += Log_Logging;
                    try
                    {
                        if (!File.Exists(strInput))
                            Log.Error($"Unable to find {strInput} project.");
                        if (!Directory.Exists(strOutput))
                            Directory.CreateDirectory(strOutput);

                        var project = new XeGsProj().Open(strInput);
                        var builder = new Builder(project, strOutput);
                        builder.OnProgress += Program_Progress;
                        if (clean) builder.Clean();
                        else builder.Build();
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

        private static void Program_Progress(string message, int filesProcessed, int filesToProcess, bool hasFinish)
        {
            var str = string.Format("[{0}/{1}] {2}", filesProcessed.ToString("D03"), filesToProcess.ToString("D03"));
            Log.Message(str);
        }

        private static void Log_Logging(Log.Level level, string message, string member, string sourceFile, int sourceLine)
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
