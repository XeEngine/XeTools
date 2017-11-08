using System;
using System.Collections.Generic;

namespace Xe.Tools.Modules
{
    public interface IModule
    {
        /// <summary>
        /// File loaded from the current module
        /// </summary>
        string InputFileName { get; }

        /// <summary>
        /// Parameters of the module
        /// </summary>
        Tuple<string, string>[] Parameters { get; }

        /// <summary>
        /// Check if loaded file is valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Obtain a list of files used from FileName.
        /// FileName itself is included.
        /// </summary>
        IEnumerable<string> InputFileNames { get; }

        /// <summary>
        /// Obtain a list of output files.
        /// FileName itself is included.
        /// </summary>
        IEnumerable<string> OutputFileNames { get; }

        /// <summary>
        /// Process the file and exports it.
        /// </summary>
        void Build();
        
        /// <summary>
        /// Clean all the used files
        /// </summary>
        void Clean();
    }
}
