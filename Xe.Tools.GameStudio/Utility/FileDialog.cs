using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.GameStudio.Utility
{
	public static class FileDialog
	{
		public enum Behavior
		{
			Open, Save
		}
		public enum Type
		{
            Any,
			XeGameProject
		}

		public static Microsoft.Win32.FileDialog Factory(Behavior behavior, Type type) {
			Microsoft.Win32.FileDialog fd;
			switch (behavior)
			{
				case Behavior.Open:
					fd = new Microsoft.Win32.OpenFileDialog();
					fd.CheckFileExists = true;
					break;
				case Behavior.Save:
					fd = new Microsoft.Win32.SaveFileDialog();
					break;
				default:
					throw new ArgumentException("Invalid parameter", nameof(behavior));
			}
			fd.CheckPathExists = true;

            string filter;
			switch (type)
			{
                case Type.Any:
                    filter = "Any file|*.*";
                    break;
				case Type.XeGameProject:
					filter = "XeEngine game project|*.game.proj.json";
					break;
                default:
                    filter = null;
                    break;
			}
            fd.Filter = filter;

			return fd;
		}
	}
}
