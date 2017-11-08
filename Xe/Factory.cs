using System;
using System.Collections.Generic;
using System.Linq;

namespace Xe
{
    public static class Factory
    {
        public enum Scope
        {
            Lifetime,
            Instance,
        }

        private struct Entry
        {
            public Type Implementation;
            public Scope Scope;
            public object Instance;
        }

        private static Dictionary<Type, Entry> _dictionary = new Dictionary<Type, Entry>();

        public static void Register<Interface, Implementation>(Scope scope)
        {
            _dictionary.Add(typeof(Interface), new Entry()
            {
                Implementation = typeof(Implementation),
                Scope = scope
            });
        }

        public static T Resolve<T>() where T : class
        {
            var entry = _dictionary[typeof(T)];
            switch (entry.Scope)
            {
                case Scope.Lifetime:
                    return (T)(entry.Instance = entry.Instance ?? Activator.CreateInstance(entry.Implementation));
                case Scope.Instance:
                    return (T)Activator.CreateInstance(entry.Implementation);
                default:
                    return null;
            }
        }
    }
}
