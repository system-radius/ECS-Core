using System;

namespace ECS.Core.Entities
{
    public readonly struct Entity : IEquatable<Entity>
    {
        public readonly int Index;
        public readonly int Version;
        public Entity(int id, int version) { Index = id; Version = version; }

        public bool Equals(Entity other)
        {
            return Index == other.Index && Version == other.Version;
        }

        public override bool Equals(object o)
        {
            return o is Entity other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Index, Version);
        }

        public override string ToString()
        {
            return $"({Index}:{Version})";
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !left.Equals(right);
        }
    }
}
