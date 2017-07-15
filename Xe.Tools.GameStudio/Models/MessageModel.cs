using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.GameStudio.Models
{
    /// <summary>
    /// Type of message
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Used during application initialization
        /// </summary>
        Initialization,

        Idle,

        Processing,

        Warning,

        Error
    }

    /// <summary>
    /// Centralize the way how message are delivered
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// Type of message
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// Message itself
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Used to take track of progress; from 0 to 1.
        /// </summary>
        public float Progress { get; set; }
    }
}
