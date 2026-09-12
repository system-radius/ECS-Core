using System.Collections.Generic;

namespace ECS.Core.Entities
{
    public sealed class EntityManager
    {
        private int nextId = 1;
        private readonly Stack<int> freeIndices = new();
        private readonly List<int> versions = new();
        private readonly List<bool> alive = new();
        public Entity Create()
        {
            int index;
            if (freeIndices.Count > 0)
            {
                index = freeIndices.Pop();
            }
            else
            {
                index = nextId++;
                versions.Add(1);
                alive.Add(false);
            }

            alive[index] = true;
            var entity = new Entity(index, versions[index]);

            return entity;
        }

        public bool Exists(Entity entity) {
            return alive[entity.Index];
        }

        public bool Destroy(Entity entity)
        {
            if (!Exists(entity)) return false;

            alive[entity.Index] = false;
            versions[entity.Index]++;
            freeIndices.Push(entity.Index);

            return true;
        }
    }
}
