using ECS.Core.Commands;
using ECS.Core.Components;
using ECS.Core.Entities;
using ECS.Core.Events;
using ECS.Core.Queries;
using ECS.Core.Resources;
using ECS.Core.Systems;

namespace ECS.Core
{
    public sealed class World
    {
        public EntityManager Entities { get; }
        public ComponentRegistry Components { get; }
        public ResourceRegistry Resources { get; }
        public QueryRegistry Queries { get; }
        public EventBus Events { get; }
        public CommandBuffer Commands { get; }
        public SystemScheduler Systems { get; }
        public World()
        {
            Entities = new EntityManager();
            Components = new ComponentRegistry();
            Resources = new ResourceRegistry();
            Queries = new QueryRegistry(Components);
            Events = new EventBus();
            Commands = new CommandBuffer();
            Systems = new SystemScheduler();
        }
    }
}
