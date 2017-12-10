using System.IO;

namespace Xe.Game.Tilemaps
{
    public interface IObjectExtension
    {
        int GetId();

        int GetLength();

        void Write(BinaryWriter writer);
    }

    public class ObjectEntry
    {
        #region Properties

        public string Name { get; set; }

        public string Type { get; set; }

        public IObjectExtension Extension { get; set; }

        #endregion

        #region Appearance

        public string AnimationData { get; set; }

        public string AnimationName { get; set; }

        public Direction Direction { get; set; }

        public bool Visible { get; set; }

        public bool HasShadow { get; set; }

        #endregion

        #region Layout

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public Flip Flip { get; set; }

        #endregion
    }
}
