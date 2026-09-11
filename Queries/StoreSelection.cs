using ECS.Core.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.Core.Queries
{
    public readonly struct StoreSelection
    {
        public readonly IComponentStore Primary;
        public readonly IComponentStore[] Secondaries;

        public StoreSelection(IComponentStore primary, IComponentStore[] secondaries)
        {
            Primary = primary; Secondaries = secondaries;
        }
    }
}
