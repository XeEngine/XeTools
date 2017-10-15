using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xe.Tools.Modules
{
    public abstract class ModuleBase : IModule
    {
        private ModuleInit Init { get; }

        public string InputFileName => Init.FileName;
		public string OutputFileName { get; private set; }

        public string InputWorkingPath => Init.InputPath;
        public string OutputWorkingPath => Init.OutputPath;

        public Tuple<string, string>[] Parameters => Init.Parameters
            .Select(x => new Tuple<string, string>(x.Key, x.Value))
            .ToArray();
        public bool IsValid { get; private set; }
        public IEnumerable<string> InputFileNames { get; private set; }
        public IEnumerable<string> OutputFileNames { get; private set; }

		public ModuleBase(ModuleInit init)
        {
            Init = init;

            var inputFileName = Path.Combine(InputWorkingPath, InputFileName);
			if (File.Exists(inputFileName))
            {
				try
                {
                    if (OpenFileData(inputFileName))
                    {
                        if (!string.IsNullOrEmpty(init.OutputFileName))
                            OutputFileName = init.OutputFileName;
                        else
                            OutputFileName = GetOutputFileName();

                        InputFileNames = GetSecondaryInputFileNames()
                            .Union(new[] { InputFileName });
                        OutputFileNames = GetSecondaryOutputFileNames()
                            .Union(new[] { OutputFileName });
                        IsValid = true;
                    }
                    else
                    {
                        Log.Error($"File {inputFileName} is not recognized as valid.");
                    }
                }
				catch (Exception e)
                {
                    Log.Error($"File {inputFileName} rasied an exception: {e.Message}");
                }
            }
			else
            {
                IsValid = false;
                Log.Error($"File {inputFileName} does not exists.");
            }
        }

		public void Build()
        {
            // Check if files are already up to date.
            var lastInputModify = GetLastInputModifyDate();
            var lastOutputModify = GetLastOutputModifyDate();
			if (lastInputModify > lastOutputModify) {
				foreach (var directory in OutputFileNames
					.Select(x => Path.GetDirectoryName(x))
					.Distinct())
                {
                    var fullPath = Path.Combine(OutputWorkingPath, directory);
                    if (!Directory.Exists(fullPath))
                        Directory.CreateDirectory(fullPath);
                }
                Export();
            }
			else
            {
                Log.Message($"Skipping {InputFileName} because it is already up to date.");
            }
        }

        public void Clean()
        {
            foreach (var x in OutputFileNames)
            {
                string fileName = Path.Combine(OutputWorkingPath, x);
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    Log.Message($"File {fileName} deleted.");
                }
            }
        }

        private DateTime GetAssemblyModifyDate()
        {
            return File.GetLastWriteTimeUtc(Init.Type.Assembly.Location);
        }

        private DateTime GetLastInputModifyDate()
        {
            var filesLastModifyDate = InputFileNames.Select(x =>
            {
                string fileName = Path.Combine(InputWorkingPath, x);
                if (File.Exists(fileName))
                {
                    return File.GetLastWriteTimeUtc(fileName);
                }
                else
                {
                    Log.Warning($"File {fileName} was not found.");
                    return DateTime.MinValue;
                }
            }).Max();
            var assemblyModifyDate = GetAssemblyModifyDate();
            if (assemblyModifyDate.Ticks > filesLastModifyDate.Ticks)
                return assemblyModifyDate;
            return filesLastModifyDate;
        }

        private DateTime GetLastOutputModifyDate()
        {
            return OutputFileNames.Select(x =>
            {
                string fileName = Path.Combine(OutputWorkingPath, x);
                if (File.Exists(fileName))
                    return File.GetLastWriteTimeUtc(fileName);
                else
                    return DateTime.MinValue;
            }).Min();
        }

        public virtual bool OpenFileData(string fileName)
        {
            using (var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return OpenFileData(fStream);
            }
        }

        public abstract bool OpenFileData(FileStream stream);

        public abstract string GetOutputFileName();

        public abstract IEnumerable<string> GetSecondaryInputFileNames();

        public abstract IEnumerable<string> GetSecondaryOutputFileNames();

        public abstract void Export();
    }
}
