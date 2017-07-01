using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;

namespace Xe.Tools.GameStudio.Utility
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
			XeGameProject
		}

        private CommonFileDialog _fd;
        public Behavior CurrentBehavior { get; private set; }
        public Type CurrentType { get; private set; }
        public string FileName
        {
            get => _fd.FileName;
        }
        public IEnumerable<string> FileNames
        {
            get => (_fd as CommonOpenFileDialog)?.FileNames ?? new string[] { FileName };
        }

        private FileDialog(CommonFileDialog commonFileDialog, Behavior behavior, Type type)
        {
            _fd = commonFileDialog;
            CurrentBehavior = behavior;
            CurrentType = type;
        }

        public bool? ShowDialog()
        {
            switch (_fd.ShowDialog())
            {
                case CommonFileDialogResult.Ok: return true;
                case CommonFileDialogResult.None: return false;
                case CommonFileDialogResult.Cancel: return null;
                default: return null;
            }
        }

		public static FileDialog Factory(Behavior behavior, Type type = Type.Any, bool multipleSelection = false) {
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
                    default:
                        break;
                }
            }

            return new FileDialog(fd, behavior, type);
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
