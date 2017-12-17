using System;
using System.IO;

namespace Xe.Game.Tilemaps
{
    public interface IObjectExtension
    {
        Guid Id { get; }

        void Write(BinaryWriter writer);
    }

    public class ObjectExtensionDefinition
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Type Type { get; set; }
    }
}
