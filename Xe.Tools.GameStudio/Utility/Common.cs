using Xe.Tools.GameStudio.Models;
using Xe.Tools.Projects;

namespace Xe.Tools.GameStudio.Utility
{
    internal static class Common
    {
        internal delegate void MessageFunc(MessageModel message);
        internal static event MessageFunc OnMessage;

        internal static void Initialize()
        {
        }

        internal static void ProjectBuild(IProject project, string outputFolder)
        {
            var builder = new Builder.Builder(project, outputFolder);
            builder.OnProgress += Asd_OnProgress;
            builder.Build();
        }

        internal static void ProjectClean(IProject project, string outputFolder)
        {
            var builder = new Builder.Builder(project, outputFolder);
            builder.OnProgress += Asd_OnProgress;
            builder.Clean();
        }

        internal static void SendMessage(MessageModel message)
        {
            OnMessage?.Invoke(message);
        }
        internal static void SendMessage(MessageType type, string message)
        {
            SendMessage(new MessageModel()
            {
                Type = type,
                Message = message
            });
        }


        private static void Asd_OnProgress(string message, int filesProcessed, int filesToProcess, bool hasFinish)
        {
            OnMessage?.Invoke(new MessageModel()
            {
                Message = message,
                Type = hasFinish ? MessageType.Idle : MessageType.Processing,
                Progress = (float)filesToProcess / filesProcessed
            });
        }
    }
}
