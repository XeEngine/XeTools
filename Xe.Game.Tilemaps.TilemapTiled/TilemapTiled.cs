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

        #region Utilities

        private static T GetPropertyValue<T>(Tiled.PropertiesDictionary properties, T defaultValue = default(T), [CallerMemberName] string key = null)
        {
            T result;
            if (properties != null && properties.TryGetValue(key, out object value))
            {
                if (typeof(T) == value.GetType())
                    return (T)value;
                try
                {
                    bool isNumeric = IsNumeric(value);
                    var type = typeof(T);
                    if (type.IsEnum)
                    {
                        if (!isNumeric)
                        {
                            foreach (var enumValues in type.GetEnumValues())
                            {
                                if (value.ToString().Equals(enumValues.ToString(), StringComparison.OrdinalIgnoreCase))
                                    return (T)enumValues;
                            }
                            return int.TryParse(value.ToString(), out int r) ? (T)(object)r : defaultValue;
                        }
                        return (T)(object)Convert.ToInt32(value);
                    }
                    result = (T)Convert.ChangeType(value, type, null);
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

        private static bool IsNumeric(object obj)
        {
            return obj is Byte || obj is SByte ||
                obj is Int16 || obj is UInt16 ||
                obj is Int32 || obj is UInt32 ||
                obj is Int64 || obj is UInt64;
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
