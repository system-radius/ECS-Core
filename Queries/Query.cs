using ECS.Core.Components;
using System;

namespace ECS.Core.Queries
{
    public sealed class Query<T> where T : struct
    {
        private readonly IComponentStore primary;
        private readonly IComponentStore[] secondaries;

        internal Query(ComponentRegistry components)
        {
            var store = components.GetStore<T>();
            primary = store;
            secondaries = Array.Empty<IComponentStore>();
        }

        public int Count => primary.Count;
        public QueryEnumerator GetEnumerator()
        {
            return new QueryEnumerator(primary, secondaries);
        }
    }

    public sealed class Query<T1, T2> where T1 : struct where T2 : struct
    {
        private readonly IComponentStore primary;
        private readonly IComponentStore[] secondaries;
        internal Query(ComponentRegistry components)
        {
            var store1 = components.GetStore<T1>();
            var store2 = components.GetStore<T2>();
            var selection = StoreSelector.Select(new IComponentStore[] { store1, store2 });
            primary = selection.Primary;
            secondaries = selection.Secondaries;
        }

        public int Count => primary.Count;
        public QueryEnumerator GetEnumerator()
        {
            return new QueryEnumerator(primary, secondaries);
        }
    }

    public sealed class Query<T1, T2, T3> where T1 : struct where T2 : struct where T3 : struct
    {
        private readonly IComponentStore primary;
        private readonly IComponentStore[] secondaries;
        internal Query(ComponentRegistry components)
        {
            var store1 = components.GetStore<T1>();
            var store2 = components.GetStore<T2>();
            var store3 = components.GetStore<T3>();
            var selection = StoreSelector.Select(new IComponentStore[] { store1, store2, store3 });
            primary = selection.Primary;
            secondaries = selection.Secondaries;
        }

        public int Count => primary.Count;
        public QueryEnumerator GetEnumerator()
        {
            return new QueryEnumerator(primary, secondaries);
        }
    }
}
