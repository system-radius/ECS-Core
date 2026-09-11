using System;
using System.Collections.Generic;

namespace ECS.Core.Resources
{
    public sealed class ResourceRegistry
    {
        private readonly Dictionary<Type, object> resources = new();
        public int Count => resources.Count;
        public void Add<T>(T resource)
        {
            Type type = typeof(T);
            if (resources.ContainsKey(type)) throw new InvalidOperationException($"Reource {type.Name} alread exists");
            resources.Add(type, resource);
        }

        public void Set<T> (T resource)
        {
            if (Has<T>())
            {
                resources[typeof(T)] = resource;
                return;
            }

            Add<T>(resource);
        }

        public bool Has<T>()
        {
            return resources.ContainsKey(typeof(T));
        }

        public T Get<T>()
        {
            if (!resources.TryGetValue(typeof(T), out var resource)) throw new InvalidOperationException($"Key {typeof(T).Name} does not exist");
            return (T)resources[typeof(T)];
        }

        public bool TryGet<T>(out T resource)
        {
            if (resources.TryGetValue(typeof(T), out var obj))
            {
                resource = (T)obj;
                return true;
            }

            resource = default(T);
            return false;
        }

        public bool Remove<T>()
        {
            return resources.Remove(typeof(T));
        }

        public void Clear()
        {
            resources.Clear();
        }

    }
}
