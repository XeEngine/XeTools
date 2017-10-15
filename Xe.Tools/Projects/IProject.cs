using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Xe.Tools.Projects
{
    public interface IProject
    {
        /// <summary>
        /// Full name of the project
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Name of project's file
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Where the project is located
        /// </summary>
        string WorkingDirectory { get; }

        /// <summary>
        /// Full path of project's file
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Short name of the project
        /// </summary>
        string ShortName { get; set; }

        /// <summary>
        /// Version of the project
        /// </summary>
        Version Version { get; }

        string Company { get; set; }

        string Producer { get; set; }

        string Copyright { get; set; }

        int Year { get; set; }

        IEnumerable<IProjectEntry> GetEntries();

        void SaveChanges();

        void SaveChanges(Stream stream);
    }
}
