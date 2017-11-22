using System;
using System.Collections.Generic;
using System.Text;

namespace Tiled
{
    public interface ILayerEntry : IEntry
    {
        bool Visible { get; set; }

        PropertiesDictionary Properties { get; }

        void SaveChanges();
    }
}
