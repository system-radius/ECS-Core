namespace ECS.Core.Events
{
    internal interface IEventStream
    {
        void Dispatch();
        void Clear();
    }
}
