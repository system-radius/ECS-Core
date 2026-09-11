using System;
using System.Collections.Generic;

namespace ECS.Core.Events
{
    public sealed class EventBus
    {
        private readonly Dictionary<Type, IEventStream> streams = new();
        public EventBus()
        {

        }

        public void Publish<T>(T evt)
        {
            GetStream<T>().Publish(evt);
        }

        public void Subscribe<T>(Action<T> callback)
        {
            GetStream<T>().Subscribe(callback);
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            GetStream<T>().Unsubscribe(callback);
        }

        public void Dispatch()
        {
            foreach (var stream in streams.Values)
            {
                stream.Dispatch();
            }
        }

        public void Clear()
        {
            foreach (var stream in streams.Values)
            {
                stream.Clear();
            }
        }

        private EventStream<T> GetStream<T>()
        {
            var type = typeof(T);
            if (streams.TryGetValue(type, out var existing)) return (EventStream<T>)existing;

            var stream = new EventStream<T>();
            streams.Add(type, stream);
            return stream;
        }
    }
}
