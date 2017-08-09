using System;
using System.Linq;
using Xe.Game.Animations;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class AnimationMappingViewModel
    {
        public AnimationRef AnimationReference { get; private set; }

        public string Name
        {
            get => AnimationReference.Name;
            set => AnimationReference.Name = value;
        }

        public string Animation
        {
            get => AnimationReference.Animation;
            set => AnimationReference.Animation = value;
        }

        public Tuple<Direction, string> Direction
        {
            get => new Tuple<Direction, string>(AnimationReference.Direction, AnimationReference.Direction.ToString());
            set => AnimationReference.Direction = value.Item1;
        }

        public bool IsDiagonal
        {
            get => AnimationReference.IsDiagonal;
            set => AnimationReference.IsDiagonal = value;
        }

        public bool FlipX
        {
            get => AnimationReference.FlipX;
            set => AnimationReference.FlipX = value;
        }

        public bool FlipY
        {
            get => AnimationReference.FlipY;
            set => AnimationReference.FlipY = value;
        }

        public AnimationMappingViewModel(AnimationRef animationsRef)
        {
            AnimationReference = animationsRef;
        }

        public override string ToString()
        {
            /*var d = AnimationReference.Direction;
            if (d != Game.Animations.Direction.Undefined)
            {
                switch (d)
                {
                    case Game.Animations.Direction.Undefined:
                        break;
                    case Game.Animations.Direction.Up:
                        break;
                    case Game.Animations.Direction.Down:
                        break;
                    case Game.Animations.Direction.Left:
                        break;
                    case Game.Animations.Direction.Right:
                        break;
                }
                return $"{Name} - {d.ToString().ToLower()}";
            }
            else
            {
                return Name;
            }*/
            return $"{Name} {GetCharacterFromDirection(AnimationReference.Direction)}";
        }

        private char GetCharacterFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Game.Animations.Direction.Undefined: return '■';
                case Game.Animations.Direction.Up: return '▲';
                case Game.Animations.Direction.Down: return '▼';
                case Game.Animations.Direction.Left: return '◀';
                case Game.Animations.Direction.Right: return '▶';
                //case Game.Animations.Direction.UpLeft: return '◤';
                //case Game.Animations.Direction.DownLeft: return '◣';
                //case Game.Animations.Direction.UpRight: return '◥';
                //case Game.Animations.Direction.DownRight: return '◢';
                default: return '?';
            }
        }
    }
}
