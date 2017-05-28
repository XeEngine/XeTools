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
			switch (type)
			{
				case Type.XeGameProject:
					fd.Filter = "XeEngine game project|*.game.proj.json";
					break;
			}
			return fd;
		}
	}
}
