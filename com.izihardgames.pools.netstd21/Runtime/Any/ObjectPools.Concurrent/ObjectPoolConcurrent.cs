using System;
using System.Collections.Concurrent;
using IziHardGames.Pools.Abstractions.NetStd21;

namespace IziHardGames.Runtime.Any.ObjectPools.Concurrent
{
    public class ObjectPoolConcurrent<T> : IPoolObjects<T> where T : class, new()
    {
        private readonly ConcurrentBag<T> values = new ConcurrentBag<T>();
        public T Rent()
        {
            if (values.TryTake(out var value))
            {
                return value;
            }
            else return new T();
        }

        public void Return(T item)
        {
            values.Add(item);
        }
    }
}
