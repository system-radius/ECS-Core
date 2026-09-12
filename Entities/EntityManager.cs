using System.Collections.Generic;

namespace ECS.Core.Entities
{
    public sealed class EntityManager
    {
        private int nextId = 0;
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
            // First, bind the index such that it is within range of expected values.
            // Then check if the entity is alive and the version is correct.
            return entity.Index >= 0 && entity.Index < alive.Count
                && alive[entity.Index] && versions[entity.Index] == entity.Version;
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
