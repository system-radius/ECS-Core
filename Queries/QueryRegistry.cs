using ECS.Core.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.Core.Queries
{
    public sealed class QueryRegistry
    {
        private readonly ComponentRegistry components;
        private readonly Dictionary<QueryDescriptor, object> queries = new();

        public QueryRegistry(ComponentRegistry components)
        {
            this.components = components ?? throw new ArgumentNullException(nameof(components)); 
        }

        public void Clear()
        {
            queries.Clear();
        }

        public Query<T> Get<T>() where T : struct
        {
            var descriptor = new QueryDescriptor(new[] { typeof(T) });
            if (queries.TryGetValue(descriptor, out var q)) return (Query<T>)q;
            var query = new Query<T>(components);
            queries.Add(descriptor, query);
            return query;
        }

        public Query<T1, T2> Get<T1, T2>() where T1 : struct where T2 : struct
        {
            var descriptor = new QueryDescriptor(new[] { typeof(T1), typeof(T2) });
            if (queries.TryGetValue(descriptor, out var q)) return (Query<T1, T2>)q;
            var query = new Query<T1, T2>(components);
            queries.Add(descriptor, query);
            return query;
        }

        public Query<T1, T2, T3> Get<T1, T2, T3>() where T1 : struct where T2 : struct where T3 : struct
        {
            var descriptor = new QueryDescriptor(new[] { typeof(T1), typeof(T2), typeof(T3) });
            if (queries.TryGetValue(descriptor, out var q)) return (Query<T1, T2, T3>)q;
            var query = new Query<T1, T2, T3>(components);
            queries.Add(descriptor, query);
            return query;
        }
    }
}
