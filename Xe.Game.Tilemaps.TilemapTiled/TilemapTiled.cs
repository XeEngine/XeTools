using System;
using System.Runtime.CompilerServices;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled : ITileMap
    {
        public TilemapTiled(string filename) :
            this(new Tiled.Map(filename))
        { }

        #region Factory pattern
        
        #endregion

        #region Utilities

        private static T GetPropertyValue<T>(Tiled.PropertiesDictionary properties, T defaultValue = default(T), [CallerMemberName] string key = null)
        {
            T result;
            if (properties != null && properties.TryGetValue(key, out string str))
            {
                try
                {
                    result = (T)Convert.ChangeType(str, typeof(T), null);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            else
            {
                result = defaultValue;
            }
            return result;
        }

        private static void SetPropertyValue<T>(Tiled.PropertiesDictionary properties, T value, [CallerMemberName] string key = null)
        {
            if (properties != null)
            {
                properties[key] = value?.ToString();
            }
        }

        #endregion
    }
}
