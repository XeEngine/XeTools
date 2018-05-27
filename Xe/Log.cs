using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Xe
{
	public delegate void LogClear();
	public delegate void LogFunc(Log.Level level, string message, string member, string sourceFile, int sourceLine);

	public static class Log
    {
        public enum Level
        {
            Error, Warning, Message
        }

		public static event LogFunc OnLog;
		public static event LogClear OnLogClear;

        public static void Error(string str,
            [CallerMemberName] string member = null,
            [CallerFilePath] string sourceFilePat = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (OnLog != null)
            {
                lock (OnLog)
                {
                    OnLog.Invoke(Level.Error, $"{str}\n", member, sourceFilePat, sourceLineNumber);
                }
            }
        }

        public static void Warning(string str,
            [CallerMemberName] string member = null,
            [CallerFilePath] string sourceFilePat = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (OnLog != null)
            {
                lock (OnLog)
                {
                    OnLog.Invoke(Level.Warning, $"{str}\n", member, sourceFilePat, sourceLineNumber);
                }
            }
        }

        public static void Message(string str,
            [CallerMemberName] string member = null,
            [CallerFilePath] string sourceFilePat = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (OnLog != null)
            {
                lock (OnLog)
                {
                    OnLog.Invoke(Level.Message, $"{str}\n", member, sourceFilePat, sourceLineNumber);
                }
            }
        }

		public static void Clear()
		{
			OnLogClear.Invoke();
		}
    }
}
