using System;
using ECS.Core.Entities;

namespace ECS.Core.Components
{
    public sealed class SparseSet<T> where T : struct
    {
        private const int InvalidIndex = -1;
        private Entity[] denseEntities;
        private T[] denseComponents;
        private int[] sparse;

        private int count;
        public int Count => count;

        public SparseSet(int initialDenseCap = 64, int initialSparseCap = 64)
        {
            denseEntities = new Entity[initialDenseCap];
            denseComponents = new T[initialDenseCap];
            sparse = new int[initialSparseCap];
            Array.Fill(sparse, InvalidIndex);
        }

        public bool Has(Entity entity) {
            int id = entity.Index;
            if (id < 0 || id >= sparse.Length) return false;

            int denseIndex = sparse[id];
            if (denseIndex == InvalidIndex) return false;

            return denseIndex < count && denseEntities[denseIndex] == entity;
        }

        public void Add(Entity entity, T component)
        {
            if (Has(entity)) throw new InvalidOperationException($"Entity {entity} already contains {typeof(T).Name}");
            int id = entity.Index;
            EnsureSparseCapacity(id);
            EnsureDenseCapacity();
            denseEntities[count] = entity;
            denseComponents[count] = component;
            sparse[id] = count;
            count++;
        }

        public void Set(Entity entity, T component)
        {
            if (Has(entity))
            {
                Get(entity) = component;
                return;
            }
            Add(entity, component);
        }

        public ref T Get(Entity entity)
        {
            if (!Has(entity)) throw new InvalidOperationException($"Entity {entity} does not exist");
            int denseIndex = sparse[entity.Index];
            return ref denseComponents[denseIndex];
        }

        public bool Remove(Entity entity)
        {
            if (!Has(entity)) return false;
            int id = entity.Index;
            int removedIndex = sparse[id];
            int lastIndex = count - 1;
            if (removedIndex != lastIndex)
            {
                Entity moved = denseEntities[lastIndex];
                denseEntities[removedIndex] = moved;
                denseComponents[removedIndex] = denseComponents[lastIndex];
                sparse[moved.Index] = removedIndex;
            }

            denseEntities[lastIndex] = default;
            denseComponents[lastIndex] = default;
            sparse[id] = InvalidIndex;
            count--;
            return true;
        }

        public Entity EntityAt(int denseIndex)
        {
            if ((uint)denseIndex >= (uint)count) throw new ArgumentOutOfRangeException(nameof(denseIndex));
            return denseEntities[denseIndex];
        }

        public ref T ComponentAt(int denseIndex)
        {
            if ((uint)denseIndex >= (uint)count) throw new ArgumentOutOfRangeException(nameof(denseIndex));
            return ref denseComponents[denseIndex];
        }

        public void Clear()
        {
            for (int i = 0; i < count; i++)
            {
                int id = denseEntities[i].Index;
                if (id >= 0 && id < sparse.Length) sparse[id] = InvalidIndex;
            }

            Array.Clear(denseEntities, 0, count);
            Array.Clear(denseComponents, 0, count);
            count = 0;
        }

        private void EnsureDenseCapacity()
        {
            if (count < denseEntities.Length) return;
            int newCapacity = Math.Max(4, denseEntities.Length * 2);
            Array.Resize(ref denseEntities, newCapacity);
            Array.Resize(ref denseComponents, newCapacity);
        }

        private void EnsureSparseCapacity(int id)
        {
            if (id < sparse.Length) return;
            int newSize = sparse.Length;

            while (newSize <= id)
            {
                newSize *= 2;
            }

            int oldLength = sparse.Length;
            Array.Resize(ref sparse, newSize);
            for (int i = oldLength; i < newSize; i++)
            {
                sparse[i] = InvalidIndex;
            }
        }
    }
}
