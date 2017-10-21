namespace Xe.Game.Animations
{
    public class AnimationDefinition
    {
        public string Name { get; set; }

        public AnimationReference Default { get; set; }

        public AnimationReference DirectionUp { get; set; }

        public AnimationReference DirectionRight { get; set; }

        public AnimationReference DirectionDown { get; set; }

        public AnimationReference DirectionLeft { get; set; }
    }
}
