namespace Xe.Game.Tilemaps
{
    public interface IObjectEntry
    {
        #region Properties

        string Name { get; set; }

        string Type { get; set; }

        #endregion

        #region Appearance

        string AnimationData { get; set; }

        string AnimationName { get; set; }
        
        Direction Direction { get; set; }

        bool Visible { get; set; }

        bool HasShadow { get; set; }

        #endregion

        #region Layout

        double X { get; set; }

        double Y { get; set; }

        double Z { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        Flip Flip { get; set; }

        #endregion
    }
}
