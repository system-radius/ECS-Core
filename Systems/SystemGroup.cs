using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.Core.Systems
{
    internal sealed class SystemGroup
    {
        public SystemGroupId Id { get; }
        private readonly List<ISystem> systems;
        public SystemGroup(SystemGroupId id)
        {
            Id = id;
            systems = new();
        }

        public void Add(ISystem system)
        {
            systems.Add(system);
        }

        public bool Remove(ISystem system)
        {
            return systems.Remove(system);
        }

        public void Clear()
        {
            systems.Clear();
        }

        public void Update(World world, float deltaTime)
        {
            for (int i = 0; i < systems.Count; i++)
            {
                systems[i].Update(world, deltaTime);
            }
        }
    }

    public static class SystemGroups
    {
        // What does the player want to do?
        public static readonly SystemGroupId Input = new SystemGroupId("Input");

        // Anything goes here by default
        public static readonly SystemGroupId Default = new SystemGroupId("Default");

        // What happens in the world?
        public static readonly SystemGroupId Simulation = new SystemGroupId("Simulation");

        // What should no longer exist?
        public static readonly SystemGroupId Cleanup = new SystemGroupId("Cleanup");

        // How to show the result?
        public static readonly SystemGroupId Presentation = new SystemGroupId("Presentation");
    }
}
