﻿using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;

namespace Xe.Tools.Wpf.Controls
{
    public abstract partial class D2DControl
    {
        public class Direct2DResourceCache
        {
            #region attributes

            private Dictionary<string, Func<RenderTarget, object>> generators = new Dictionary<string, Func<RenderTarget, object>>();
            private Dictionary<string, object> resources = new Dictionary<string, object>();
            private RenderTarget renderTarget = null;

            #endregion

            #region properties

            public RenderTarget RenderTarget
            {
                get { return renderTarget; }
                set { renderTarget = value; UpdateResources(); }
            }

            public int Count
            {
                get { return resources.Count; }
            }

            public object this[string key]
            {
                get { return resources[key]; }
            }

            public Dictionary<string, object>.KeyCollection Keys
            {
                get { return resources.Keys; }
            }

            public Dictionary<string, object>.ValueCollection Values
            {
                get { return resources.Values; }
            }

            #endregion

            #region public methods

            public void Add(string key, Func<RenderTarget, object> gen)
            {
                if (resources.TryGetValue(key, out object resOld))
                {
                    Utility.SafeDispose(ref resOld);
                    generators.Remove(key);
                    resources.Remove(key);
                }

                if (renderTarget == null)
                {
                    generators.Add(key, gen);
                    resources.Add(key, null);
                }
                else
                {
                    var res = gen(renderTarget);
                    generators.Add(key, gen);
                    resources.Add(key, res);
                }
            }

            public void Clear()
            {
                foreach (var key in resources.Keys)
                {
                    var res = resources[key];
                    Utility.SafeDispose(ref res);
                }
                generators.Clear();
                resources.Clear();
            }

            public bool ContainsKey(string key)
            {
                return resources.ContainsKey(key);
            }

            public bool ContainsValue(object val)
            {
                return resources.ContainsValue(val);
            }

            public Dictionary<string, object>.Enumerator GetEnumerator()
            {
                return resources.GetEnumerator();
            }

            public bool Remove(string key)
            {
                if (resources.TryGetValue(key, out object res))
                {
                    Utility.SafeDispose(ref res);
                    generators.Remove(key);
                    resources.Remove(key);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool TryGetValue(string key, out object res)
            {
                return resources.TryGetValue(key, out res);
            }

            #endregion

            #region private methods

            private void UpdateResources()
            {
                if (renderTarget == null) { return; }

                foreach (var g in generators)
                {
                    var key = g.Key;
                    var gen = g.Value;
                    var res = gen(renderTarget);

                    if (resources.TryGetValue(key, out object resOld))
                    {
                        Utility.SafeDispose(ref resOld);
                        resources.Remove(key);
                    }

                    resources.Add(key, res);
                }
            }

            #endregion
        }
    }
}