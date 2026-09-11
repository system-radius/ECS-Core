using ECS.Core.Entities;
using System;

namespace ECS.Core.Components
{
    public sealed class ComponentStore<T> : IComponentStore where T : struct
    {
        public Type ComponentType => typeof(T);

        private readonly SparseSet<T> storage;
        public int Count => storage.Count;

        public ComponentStore()
        {
            storage = new();
        }

        public bool Has(Entity entity)
        {
            return storage.Has(entity);
        }

        public void Add(Entity entity, T component)
        { 
            storage.Add(entity, component);
        }

        public void Set(Entity entity, T component)
        {
            storage.Set(entity, component);
        }

        public bool Remove(Entity entity)
        {
            return storage.Remove(entity);
        }

        public ref T Get(Entity entity)
        {
            return ref storage.Get(entity);
        }

        public Entity EntityAt(int index)
        {
            return storage.EntityAt(index);
        }

        public ref T ComponentAt(int index)
        {
            return ref storage.ComponentAt(index);
        }

        public void Clear()
        {
            storage.Clear();
        }
    }
}
