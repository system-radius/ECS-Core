using ECS.Core.Entities;
using System;
using System.Collections.Generic;

namespace ECS.Core.Components
{
    public sealed class ComponentRegistry
    {
        private readonly Dictionary<Type, IComponentStore> stores = new();
        public int StoreCount => stores.Count;

        public void Register<T>() where T : struct
        {
            Type type = typeof(T);
            if (stores.ContainsKey(type)) throw new InvalidOperationException($"Component Store of type {type.Name} already exists");
            stores.Add(type, new ComponentStore<T>());
        }

        public ComponentStore<T> GetStore<T>() where T : struct
        {
            Type type = typeof(T);
            if (!stores.TryGetValue(type, out var store))
            {
                store = new ComponentStore<T>();
                stores.Add(type, store);
            }

            return (ComponentStore<T>) store;
        }

        public bool HasStore<T>() where T : struct
        {
            return stores.ContainsKey(typeof(T));
        }

        public bool Remove<T>(Entity entity) where T : struct
        {
            return GetStore<T>().Remove(entity);
        }

        public ref T Get<T>(Entity entity) where T : struct
        {
            return ref GetStore<T>().Get(entity);
        }

        public void Set<T>(Entity entity, T component) where T : struct
        {
            GetStore<T>().Set(entity, component);
        }

        public void Add<T>(Entity entity, T component) where T : struct
        {
            GetStore<T>().Add(entity, component);
        }

        public bool Has<T>(Entity entity) where T : struct
        {
            return GetStore<T>().Has(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            foreach (var store in stores.Values)
            {
                store.Remove(entity);
            }
        }
    }
}
