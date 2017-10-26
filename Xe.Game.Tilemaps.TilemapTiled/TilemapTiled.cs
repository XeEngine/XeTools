using System;
using System.Linq;
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
                    var type = typeof(T);
                    if (type.IsEnum)
                    {
                        foreach (var value in type.GetEnumValues())
                        {
                            if (str.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase))
                                return (T)value;
                        }
                        return int.TryParse(str, out int r) ? (T)(object)r : defaultValue;
                    }
                    result = (T)Convert.ChangeType(str, type, null);
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
