namespace Xe.Game.Animations
{
    public enum Direction
    {
        Up = 0, Right = 1, Down = 2, Left = 3,
        Undefined = -1
    }
    public class AnimationRef
    {
        public string Name { get; set; }

        public string Animation { get; set; }

        public Direction Direction { get; set; }

        public bool IsDiagonal { get; set; }

        public bool FlipX { get; set; }

        public bool FlipY { get; set; }
    }
}
