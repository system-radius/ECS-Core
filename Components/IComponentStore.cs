using ECS.Core.Entities;

namespace ECS.Core.Components
{
    public interface IComponentStore
    {
        System.Type ComponentType { get; }
        bool Remove(Entity entity);
        bool Has(Entity entity);
        void Clear();
        int Count { get; }
        Entity EntityAt(int index);
    }
}
