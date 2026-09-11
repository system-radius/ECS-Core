using System.Collections.Generic;

namespace ECS.Core.Entities
{
    public sealed class EntityManager
    {
        private uint nextId = 1;
        private readonly HashSet<Entity> alive = new();
        public Entity Create()
        {
            var entity = new Entity(nextId++);
            alive.Add(entity);

            return entity;
        }

        public bool Exists(Entity entity) {
            return alive.Contains(entity);
        }

        public bool Destroy(Entity entity)
        {
            return alive.Remove(entity);
        }

        public IReadOnlyCollection<Entity> Alive => alive;
    }
}
