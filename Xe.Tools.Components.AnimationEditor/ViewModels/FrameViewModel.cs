using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Xe.Game.Animations;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class FrameViewModel
    {
        public Frame Frame { get; private set; }

        public string Name
        {
            get => Frame.Name;
            set => Frame.Name = value;
        }

        public int Left
        {
            get => Frame.Left;
            set => Frame.Left = value;
        }

        public int Top
        {
            get => Frame.Top;
            set => Frame.Top = value;
        }

        public int Right
        {
            get => Frame.Right;
            set => Frame.Right = value;
        }

        public int Bottom
        {
            get => Frame.Bottom;
            set => Frame.Bottom = value;
        }

        public int CenterX
        {
            get => Frame.CenterX;
            set => Frame.CenterX = value;
        }

        public int CenterY
        {
            get => Frame.CenterY;
            set => Frame.CenterY = value;
        }

        private int _maximumWidth;
        public int MaximumWidth
        {
            get => _maximumWidth;
            set => _maximumWidth = value;
        }

        private int _maximumHeight;
        public int MaximumHeight
        {
            get => _maximumHeight;
            set => _maximumHeight = value;
        }

        public FrameViewModel(Frame frame)
        {
            Frame = frame;
        }
    }
}
