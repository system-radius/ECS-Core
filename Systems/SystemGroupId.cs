using System;

namespace ECS.Core.Systems
{
    public readonly struct SystemGroupId : IEquatable<SystemGroupId>
    {
        public string Name { get; }

        public SystemGroupId(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public bool Equals(SystemGroupId other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is SystemGroupId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(SystemGroupId left, SystemGroupId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SystemGroupId left, SystemGroupId right)
        {
            return !left.Equals(right);
        }
    }
}
