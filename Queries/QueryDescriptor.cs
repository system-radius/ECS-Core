
using System;

namespace ECS.Core.Queries
{
    internal readonly struct QueryDescriptor : IEquatable<QueryDescriptor>
    {
        public readonly Type[] Types;
        public QueryDescriptor(Type[] types)
        {
            Types = (Type[]) types.Clone();
        }

        public bool Equals(QueryDescriptor other)
        {
            if (Types.Length != other.Types.Length) return false;
            for (int i = 0; i < Types.Length; i++)
            {
                if (Types[i] != other.Types[i]) return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is QueryDescriptor other && Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < Types.Length; i++)
            {
                hash = hash * 31 + Types[i].GetHashCode();
            }

            return hash;
        }
    }
}
