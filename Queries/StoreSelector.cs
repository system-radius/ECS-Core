using System;
using ECS.Core.Components;

namespace ECS.Core.Queries
{
    internal static class StoreSelector
    {
        public static StoreSelection Select(IComponentStore[] stores)
        {
            if (stores == null) throw new ArgumentNullException(nameof(stores));
            if (stores.Length == 0) throw new ArgumentException($"Required at least one store");

            int smallestIndex = 0;
            for (int i = 1; i < stores.Length; i++)
            {
                if (stores[i].Count < stores[smallestIndex].Count) smallestIndex = i;
            }

            var primary = stores[smallestIndex];
            var secondaries = new IComponentStore[stores.Length - 1];
            int secondaryIndex = 0;
            for (int i = 0; i < stores.Length; i++)
            {
                if (i == smallestIndex) continue;
                secondaries[secondaryIndex++] = stores[i];
            }

            return new StoreSelection(primary, secondaries);
        }
    }
}
