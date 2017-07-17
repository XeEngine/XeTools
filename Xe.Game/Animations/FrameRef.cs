namespace Xe.Game.Animations
{
    public class FrameRef : IDeepCloneable
    {
        public string Frame { get; set; }

        public bool Trigger { get; set; }

        public bool FlipX { get; set; }

        public bool FlipY { get; set; }

        public Hitbox Hitbox { get; set; }

        public object DeepClone()
        {
            return new FrameRef()
            {
                Frame = Frame,
                Trigger = Trigger,
                FlipX = FlipX,
                FlipY = FlipY,
                Hitbox = Hitbox.DeepClone() as Hitbox
            };
        }
    }
}
