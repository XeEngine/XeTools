using System;
using System.Collections.Generic;
using System.Linq;

namespace Xe.Tools.Projects
{
    public static class Utility
    {
        public static IEnumerable<IProjectFile> GetFiles(this IProject project)
        {
            return GetFiles(project.GetEntries());
        }

        public static IEnumerable<IProjectFile> GetFiles(this IProjectDirectory directory)
        {
            return GetFiles(directory.GetEntries());
        }

        public static IEnumerable<IProjectFile> GetFiles(this IEnumerable<IProjectEntry> entries)
        {
            return entries
                .Where(x => x is IProjectDirectory)
                .SelectMany(x => GetFiles(x as IProjectDirectory))
                .Union(
                    entries.Where(x => x is IProjectFile)
                        .Select(x => x as IProjectFile)
                );
        }

        public static IEnumerable<IProjectFile> GetFilesByFormat(this IProject project, string format)
        {
            return GetFilesByFormat(project.GetEntries(), format);
        }

        public static IEnumerable<IProjectFile> GetFilesByFormat(this IProjectDirectory directory, string format)
        {
            return GetFilesByFormat(directory.GetEntries(), format);
        }

        public static IEnumerable<IProjectFile> GetFilesByFormat(this IEnumerable<IProjectEntry> entries, string format)
        {
            return entries
                .Where(x => x is IProjectDirectory)
                .SelectMany(x => GetFiles(x as IProjectDirectory))
                .Union(
                    entries.Where(x =>
                    {
                        if (x is IProjectFile file)
                            return file.Format == format;
                        return false;
                    }).Select(x => x as IProjectFile)
                );
        }

        public static IEnumerable<IProjectFile> GetFilesWhere(this IProject project, Func<IProjectFile, bool> predicate)
        {
            return GetFilesWhere(project.GetEntries(), predicate);
        }

        public static IEnumerable<IProjectFile> GetFilesWhere(this IProjectDirectory directory, Func<IProjectFile, bool> predicate)
        {
            return GetFilesWhere(directory.GetEntries(), predicate);
        }

        public static IEnumerable<IProjectFile> GetFilesWhere(this IEnumerable<IProjectEntry> entries, Func<IProjectFile, bool> predicate)
        {
            return entries
                .Where(x => x is IProjectDirectory)
                .SelectMany(x => GetFiles(x as IProjectDirectory))
                .Union(
                    entries.Where(x => x is IProjectFile)
                    .Select(x => x as IProjectFile)
                    .Where(predicate)
                );
        }
    }
}
