namespace ECS.Core.Systems
{
    public interface ISystem
    {
        void Update(World world, float deltaTime);
    }

    public enum GroupSequence
    {
        First,
        Last,
        Before,
        After
    }
}
