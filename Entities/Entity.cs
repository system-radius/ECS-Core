using System;

namespace ECS.Core.Entities
{
    public readonly struct Entity : IEquatable<Entity>
    {
        public readonly uint ID;
        public Entity(uint id) { this.ID = id; }

        public bool Equals(Entity other)
        {
            return ID == other.ID;
        }

        public override bool Equals(object o)
        {
            return o is Entity other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)ID;
        }

        public override string ToString()
        {
            return ID.ToString();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !left.Equals(right);
        }

        public static implicit operator int(Entity entity) => (int)entity.ID;
    }
}
