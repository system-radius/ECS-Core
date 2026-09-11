using ECS.Core.Components;
using ECS.Core.Entities;
using System.Collections.Generic;

namespace ECS.Core.Commands
{
    internal sealed class RemoveCommandQueue<T> : ICommandQueue where T : struct
    {
        private readonly List<Entity> commands = new();
        public RemoveCommandQueue() {

        }

        public void Enqueue(Entity entity)
        {
            commands.Add(entity);
        }

        public void Playback(ComponentRegistry components)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                components.Remove<T>(commands[i]);
            }
            commands.Clear();
        }

        public void Clear()
        {
            commands.Clear();
        }
    }
}
