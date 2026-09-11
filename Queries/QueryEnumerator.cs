using ECS.Core.Components;
using ECS.Core.Entities;

namespace ECS.Core.Queries
{
    public struct QueryEnumerator
    {
        private readonly IComponentStore primary;
        private readonly IComponentStore[] secondaries;
        private int index;
        private Entity current;

        internal QueryEnumerator(IComponentStore primary, IComponentStore[] secondaries)
        {
            this.primary = primary;
            this.secondaries = secondaries;
            index = -1;
            current = default;
        }

        public Entity Current => current;

        public bool MoveNext()
        {
            while (true)
            {
                index++;
                if (index >= primary.Count) return false;
                var entity = primary.EntityAt(index);
                bool valid = true;
                for (int i = 0; i < secondaries.Length; i++)
                {
                    if (!secondaries[i].Has(entity))
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid) continue;

                current = entity;
                return true;
            }
        }
    }
}
