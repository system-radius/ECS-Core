using ECS.Core.Components;
using ECS.Core.Entities;
using System.Collections.Generic;

namespace ECS.Core.Commands
{
    internal struct SetComponentCommand<T> where T : struct
    {
        public Entity Entity;
        public T Component;

        internal SetComponentCommand(Entity entity, T component)
        {
            Entity = entity;
            Component = component;
        }
    }

    internal sealed class SetCommandQueue<T> : ICommandQueue where T : struct
    {
        private readonly List<SetComponentCommand<T>> commands = new();
        public SetCommandQueue()
        {

        }

        public void Enqueue(Entity entity, T component)
        {
            commands.Add(new SetComponentCommand<T>(entity, component));
        }

        public void Playback(ComponentRegistry components)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                var cmd = commands[i];
                components.Set(cmd.Entity, cmd.Component);
            }

            commands.Clear();
        }

        public void Clear()
        {
            commands.Clear();
        }
    }
}
