using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Services
{
    /// <summary>
    /// Manages a specific type of resources
    /// </summary>
    /// <typeparam name="T">Type of resources to manage</typeparam>
    public class ResourceService<K, T> where T : class
    {
        private enum State
        {
            Uninitialized,
            Loaded,
            Error,
        }

        private class Entry
        {
            public State State;
            public T Item;
        }

        private Dictionary<K, Entry> _resources = new Dictionary<K, Entry>();
        private Load _funcLoad;
        private Dispose _funcDispose;

        public delegate bool Load(K key, out T item);
        public delegate void Dispose(K key, T item);

        public ResourceService(Load funcLoad, Dispose funcDispose)
        {
            _funcLoad = funcLoad ?? throw new ArgumentNullException(nameof(funcLoad));
            _funcDispose = funcDispose ?? throw new ArgumentNullException(nameof(funcDispose));
        }

        public void Add(K key)
        {
            _resources.Add(key, new Entry()
            {
                State = State.Uninitialized,
                Item = null
            });
        }

        public bool Exists(K key)
        {
            return _resources.ContainsKey(key);
        }

        public void LoadAll()
        {
            foreach (var items in _resources)
                OnLoad(items.Key, items.Value);
        }

        public void Cleanup()
        {
            foreach (var items in _resources)
                OnDispose(items.Key, items.Value);
        }

        public void RemoveAll()
        {
            Cleanup();
            _resources.Clear();
        }

        public T this[K key]
        {
            get
            {
                if (!_resources.TryGetValue(key, out var value))
                {
                    _resources.Add(key, value = new Entry()
                    {
                        State = State.Uninitialized,
                        Item = null
                    });
                }
                return OnLoad(key, value);
            }
        }
        
        private T OnLoad(K key, Entry entry)
        {
            lock (entry)
            {
                if (entry.State == State.Uninitialized)
                {
                    entry.State = _funcLoad(key, out entry.Item) ? State.Loaded : State.Error;
                }
                return entry.Item;
            }
        }

        private void OnDispose(K key, Entry entry)
        {
            lock (entry)
            {
                _funcDispose(key, entry.Item);
                entry.Item = null;
                entry.State = State.Uninitialized;
            }
        }
    }
}
