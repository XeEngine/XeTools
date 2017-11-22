namespace Xe.Game.Tilemaps
{
    public interface ILayerEntry : ILayerBase
    {
        int Priority { get; set; }
    }
}
