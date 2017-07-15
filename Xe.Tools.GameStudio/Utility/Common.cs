using Xe.Tools.GameStudio.Models;

namespace Xe.Tools.GameStudio.Utility
{
    internal static class Common
    {
        internal delegate void MessageFunc(MessageModel message);
        internal static event MessageFunc OnMessage;

        internal static void Initialize()
        {
            Builder.Program.OnProgress += (message, filesProcessed, filesToProcess, hasFinish) =>
            {
                OnMessage?.Invoke(new MessageModel()
                {
                    Message = message,
                    Type = hasFinish ? MessageType.Idle : MessageType.Processing,
                    Progress = (float)filesToProcess / filesProcessed
                });
            };
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
    }
}
