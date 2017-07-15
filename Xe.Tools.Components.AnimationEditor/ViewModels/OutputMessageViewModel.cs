using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    internal enum OutputMessageType
    {
        Error,
        Warning,
        Information
    }

    internal class OutputMessageViewModel
    {
        private OutputMessageType _type;
        private string _description;
        
        public string Description => _description;

        public OutputMessageViewModel(OutputMessageType type, string description)
        {
            _type = type;
            _description = description;
        }
    }
}
