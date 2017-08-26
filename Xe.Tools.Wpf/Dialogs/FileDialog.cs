using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Xe.Tools.Wpf.Dialogs
{
    public class FileDialog
    {
        public enum Behavior
        {
            Open, Save, Folder
        }
        public enum Type
        {
            Any,
            Executable,
            XeGameProject,
            XeAnimation,
            ImagePng,
        }

        private CommonFileDialog _fd;

        private Window WindowParent { get; }

        public Behavior CurrentBehavior { get; }

        public Type CurrentType { get; }

        public string FileName => _fd.FileName;

        public IEnumerable<string> FileNames => (_fd as CommonOpenFileDialog)?.FileNames ?? new string[] { FileName };

        private FileDialog(CommonFileDialog commonFileDialog, Window wndParent, Behavior behavior, Type type)
        {
            _fd = commonFileDialog;
            WindowParent = wndParent;
            CurrentBehavior = behavior;
            CurrentType = type;
        }

        public bool? ShowDialog()
        {
            switch (_fd.ShowDialog(WindowParent))
            {
                case CommonFileDialogResult.Ok: return true;
                case CommonFileDialogResult.None: return false;
                case CommonFileDialogResult.Cancel: return null;
                default: return null;
            }
        }

        public static FileDialog Factory(Window wndParent, Behavior behavior, Type type = Type.Any, bool multipleSelection = false)
        {
            CommonFileDialog fd;
            switch (behavior)
            {
                case Behavior.Open:
                    fd = new CommonOpenFileDialog()
                    {
                        EnsureFileExists = true,
                        Multiselect = multipleSelection
                    };
                    break;
                case Behavior.Save:
                    fd = new CommonSaveFileDialog()
                    {

                    };
                    break;
                case Behavior.Folder:
                    fd = new CommonOpenFileDialog()
                    {
                        IsFolderPicker = true,
                        Multiselect = multipleSelection
                    };
                    break;
                default:
                    throw new ArgumentException("Invalid parameter", nameof(behavior));
            }
            fd.AddToMostRecentlyUsedList = true;
            fd.EnsurePathExists = true;

            if (behavior != Behavior.Folder)
            {
                switch (type)
                {
                    case Type.Any:
                        fd.Filters.Add(CreateFilter("All files",
                            new string[] { "*" }));
                        break;
                    case Type.Executable:
                        fd.Filters.Add(CreateFilter("Application",
                            new string[] { "exe" }));
                        break;
                    case Type.XeGameProject:
                        fd.Filters.Add(CreateFilter("XeEngine project",
                            new string[] { "proj.json" }));
                        break;
                    case Type.XeAnimation:
                        fd.Filters.Add(CreateFilter("XeEngine project",
                            new string[] { "anim.json" }));
                        break;
                    case Type.ImagePng:
                        fd.Filters.Add(CreateFilter("PNG image file",
                            new string[] { "png" }));
                        break;
                    default:
                        break;
                }
            }

            return new FileDialog(fd, wndParent, behavior, type);
        }

        private static CommonFileDialogFilter CreateFilter(string name, string[] filters)
        {
            var filter = new CommonFileDialogFilter()
            {
                DisplayName = name
            };
            foreach (var item in filters)
                filter.Extensions.Add(item);
            return filter;
        }
    }
}
