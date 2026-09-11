using ECS.Core.Components;
using ECS.Core.Entities;
using System;
using System.Collections.Generic;

namespace ECS.Core.Commands
{
    public sealed class CommandBuffer
    {
        private readonly Dictionary<Type, ICommandQueue> addQueues = new();
        private readonly Dictionary<Type, ICommandQueue> setQueues = new();
        private readonly Dictionary<Type, ICommandQueue> removeQueues = new();
        private readonly List<Entity> destroyQueue = new();

        public CommandBuffer() { } 

        public void Add<T>(Entity entity, T component) where T : struct
        {
            GetAddQueue<T>().Enqueue(entity, component);
        }

        public void Set<T>(Entity entity, T component) where T : struct
        {
            GetSetQueue<T>().Enqueue(entity, component);
        }

        public void Remove<T>(Entity entity) where T : struct
        {
            GetRemoveQueue<T>().Enqueue(entity);
        }

        public void Destroy(Entity entity)
        {
            destroyQueue.Add(entity);
        }

        public void Playback(ComponentRegistry components, EntityManager entities)
        {
            /*
             * Playback runs in an ordered fashion: add, set, remove, and then entities destruction.
             */
            PlaybackQueue(components, addQueues);
            PlaybackQueue(components, setQueues);
            PlaybackQueue(components, removeQueues);

            for (int i = 0; i < destroyQueue.Count; i++)
            {
                var entity = destroyQueue[i];
                components.RemoveEntity(entity);
                entities.Destroy(entity);
            }
            destroyQueue.Clear();
        }

        private void PlaybackQueue(ComponentRegistry components, Dictionary<Type, ICommandQueue> queues)
        {
            foreach (var queue in queues.Values)
            {
                queue.Playback(components);
            }
        }

        private AddCommandQueue<T> GetAddQueue<T>() where T : struct
        {
            var type = typeof(T);
            if (addQueues.TryGetValue(type, out var existing)) return (AddCommandQueue<T>)existing;
            var queue = new AddCommandQueue<T>();
            addQueues.Add(type, queue);
            return queue;
        }

        private SetCommandQueue<T> GetSetQueue<T>() where T : struct
        {
            var type = typeof(T);
            if (setQueues.TryGetValue(type, out var existing)) return (SetCommandQueue<T>)existing;
            var queue = new SetCommandQueue<T>();
            setQueues.Add(type, queue);
            return queue;
        }

        private RemoveCommandQueue<T> GetRemoveQueue<T>() where T : struct
        {
            var type = typeof(T);
            if (removeQueues.TryGetValue(type, out var existing)) return (RemoveCommandQueue<T>)existing;
            var queue = new RemoveCommandQueue<T>();
            removeQueues.Add(type, queue);
            return queue;
        }

        public void Clear()
        {
            foreach (var queue in addQueues.Values) queue.Clear();
            foreach (var queue in setQueues.Values) queue.Clear();
            foreach (var queue in removeQueues.Values) queue.Clear();
            destroyQueue.Clear();
        }
    }
}
