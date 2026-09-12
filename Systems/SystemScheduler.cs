using System;
using System.Collections.Generic;

namespace ECS.Core.Systems
{
    public sealed class SystemScheduler
    {
        private readonly Dictionary<SystemGroupId, SystemGroup> groups;
        private readonly List<SystemGroup> executionOrder;

        private bool isUpdating;

        public SystemScheduler()
        {
            groups = new Dictionary<SystemGroupId, SystemGroup>();
            executionOrder = new List<SystemGroup>();
            CreateBuiltInGroups();
        }

        public void Add(ISystem system, SystemGroupId groupId)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            var group = GetGroup(groupId);
            if (group == null) throw new ArgumentOutOfRangeException(nameof(groupId), $"Group '{groupId}' not found.");
            group.Add(system);
        }

        public void Add(ISystem system)
        {
            Add(system, SystemGroups.Default);
        }

        public bool Remove(ISystem system)
        {
            foreach (var group in executionOrder)
            {
                if (group.Remove(system)) return true;
            }

            return false;
        }

        public void Clear()
        {
            foreach (var group in executionOrder) group.Clear();
        }

        public void Update(World world, float deltaTime)
        {
            if (isUpdating) throw new InvalidOperationException("Update called more than once in the same frame");

            isUpdating = true;
            try
            {
                if (world == null) throw new ArgumentNullException(nameof(world));
                for (int i = 0; i < executionOrder.Count; i++)
                {
                    executionOrder[i].Update(world, deltaTime);
                }

                world.Commands.Playback(world.Components, world.Entities);
                world.Events.Dispatch();
            }
            catch (Exception e)
            {
                // Add logging later
                throw e;
            }
            finally { isUpdating = false; }
        }

        public void CreateGroup(SystemGroupId id)
        {
            CreateGroup(id, GroupSequence.Last);
        }

        public void CreateGroup(SystemGroupId id, GroupSequence seq)
        {
            if (groups.ContainsKey(id)) return;
            var group = new SystemGroup(id);
            switch (seq)
            {
                case GroupSequence.First:
                    executionOrder.Insert(0, group);
                    break;
                case GroupSequence.Last:
                    executionOrder.Add(group);
                    break;
                default:
                    throw new InvalidOperationException($"{seq} requires a relative group");
            }
            groups.Add(id, group);
        }

        public void CreateGroup(SystemGroupId id, GroupSequence seq, SystemGroupId relativeTo)
        {
            if (groups.ContainsKey(id)) return;
            int relativeIndex = FindGroupIndex(relativeTo);
            if (relativeIndex < 0) throw new InvalidOperationException($"Group {relativeTo} does not exist");
            var group = new SystemGroup(id);
            switch (seq)
            {
                case GroupSequence.Before:
                    executionOrder.Insert(relativeIndex, group);
                    break;
                case GroupSequence.After:
                    executionOrder.Insert(relativeIndex + 1, group);
                    break;
                default:
                    throw new InvalidOperationException($"{seq} is not supported for this call");
            }
            groups.Add(id, group);
        }

        private SystemGroup GetGroup(SystemGroupId id)
        {
            if (groups.TryGetValue(id, out var group)) return group;
            return null;
        }

        private int FindGroupIndex(SystemGroupId id)
        {
            for (int i = 0; i < executionOrder.Count; i++)
            {
                if (executionOrder[i].Id == id) return i;
            }
            return -1;
        }

        private void CreateBuiltInGroups()
        {
            CreateGroup(SystemGroups.Input);
            CreateGroup(SystemGroups.Default);
            CreateGroup(SystemGroups.Simulation);
            CreateGroup(SystemGroups.Cleanup);
            CreateGroup(SystemGroups.Presentation);
        }
    }
}
