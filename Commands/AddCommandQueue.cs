using ECS.Core.Components;
using ECS.Core.Entities;
using System.Collections.Generic;

namespace ECS.Core.Commands
{
    internal struct AddComponentCommand<T> where T : struct
    {
        public Entity Entity;
        public T Component;

        internal AddComponentCommand(Entity entity, T component)
        {
            Entity = entity;
            Component = component;
        }
    }

    internal sealed class AddCommandQueue<T> : ICommandQueue where T : struct
    {
        private readonly List<AddComponentCommand<T>> commands = new();
        public AddCommandQueue()
        {

        }

        public void Enqueue(Entity entity, T component)
        {
            commands.Add(new AddComponentCommand<T>(entity, component));
        }

        public void Playback(ComponentRegistry components)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                var cmd = commands[i];
                components.Add(cmd.Entity, cmd.Component);
            }

            commands.Clear();
        }

        public void Clear()
        {
            commands.Clear();
        }
    }
}
