using System;
using System.Collections.Generic;

namespace ECS.Core.Events
{
    internal sealed class EventStream<T> : IEventStream
    {
        private readonly List<T> bufferA = new();
        private readonly List<T> bufferB = new();

        private List<T> writeBuffer;
        private List<T> readBuffer;

        private readonly List<Action<T>> subscribers = new();
        private readonly List<Action<T>> pendingAdditions = new();
        private readonly List<Action<T>> pendingRemovals = new();

        private bool isDispatching;

        public EventStream()
        {
            writeBuffer = bufferA;
            readBuffer = bufferB;
        }

        public void Publish(T evt)
        {
            writeBuffer.Add(evt);
        }

        public void Subscribe(Action<T> callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            if (isDispatching)
            {
                pendingAdditions.Add(callback);
                return;
            }

            if (subscribers.Contains(callback)) return;
            subscribers.Add(callback);
        }

        public void Unsubscribe(Action<T> callback)
        {
            if (callback == null) return;

            if (isDispatching)
            {
                pendingRemovals.Remove(callback);
                return;
            }

            subscribers.Remove(callback);
        }

        public void Dispatch()
        {
            SwapBuffers();

            if (readBuffer.Count == 0) return;

            isDispatching = true;

            for (int i = 0; i < readBuffer.Count; i++)
            {
                T evt = readBuffer[i];
                for (int j = 0; j < subscribers.Count; j++)
                {
                    subscribers[j](evt);
                }
            }

            isDispatching = false;
            ApplyPendingChanges();
            readBuffer.Clear();
        }

        public void Clear()
        {
            readBuffer.Clear();
            writeBuffer.Clear();
            subscribers.Clear();
        }

        private void ApplyPendingChanges()
        {
            for (int i = 0; i < pendingRemovals.Count; i++)
            {
                var callback = pendingRemovals[i];
                subscribers.Remove(callback);
            }
            pendingRemovals.Clear();

            for (int i = 0; i < pendingAdditions.Count; i++)
            {
                var callback = pendingAdditions[i];
                if (!subscribers.Contains(callback)) subscribers.Add(callback);
            }
            pendingAdditions.Clear();
        }

        private void SwapBuffers()
        {
            var temp = readBuffer;
            readBuffer = writeBuffer;
            writeBuffer = temp;
        }
    }
}
