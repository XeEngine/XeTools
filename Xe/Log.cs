using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe
{
    public delegate void LogFunc(Log.Level level, string message);

    public static class Log
    {
        public enum Level
        {
            Error, Warning, Message
        }
        public static event LogFunc OnLog;
        public static void Error(string str)
        {
            if (OnLog != null)
                lock (OnLog)
                    OnLog.Invoke(Level.Error, $"{str}\n");
        }
        public static void Warning(string str)
        {
            if (OnLog != null)
                lock (OnLog)
                    OnLog.Invoke(Level.Warning, $"{str}\n");
        }
        public static void Message(string str)
        {
            if (OnLog != null)
                lock (OnLog)
                    OnLog.Invoke(Level.Message, $"{str}\n");
        }
    }
}
