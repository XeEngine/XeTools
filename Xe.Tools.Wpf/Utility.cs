using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Wpf
{
    public static class Utility
    {
        public static void SafeDispose<T>(ref T resource) where T : class
        {
            if (resource == null)
            {
                return;
            }

            if (resource is IDisposable disposer)
            {
                try
                {
                    disposer.Dispose();
                }
                catch
                {
                }
            }

            resource = null;
        }
    }
}
