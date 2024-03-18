using System;
using System.Collections.Generic;
using IziHardGames.Apps.Abstractions.Lib;
using Handler = System.Action<object>;

namespace IziHardGames.Apps.Abstractions.NetStd21
{
    /// <summary>
    /// Сопоставляет глобальный обработчик для типа
    /// </summary>
    [StaticAbstraction]
    public static class IziHandler
    {
        public static readonly HandlerSelector selector = new HandlerSelector();

		public static void CleanupStatic()
		{
            selector.Cleanup();
        }
	}

    public class HandlerSelector
    {
        private readonly Dictionary<Type, Handler> pairs = new Dictionary<Type, Handler>();
        public Handler this[Type type] { get => pairs[type]; set => AddOrUpdate(type, value); }
        public IEnumerable<Handler> All => pairs.Values;
        private void AddOrUpdate(Type type, Handler value)
        {
            if (pairs.TryGetValue(type, out var existed))
            {
                pairs[type] = value;
            }
            else
            {
                pairs.Add(type, value);
            }
            if (value == null) throw new NullReferenceException("Value handler is null");
        }
        public T As<T>() where T : HandlerSelector
        {
            return this as T ?? throw new InvalidCastException();
        }
        public Handler? ValueOrNull(Type t)
        {
            if (pairs.TryGetValue(t, out var value)) return value;
            return null;
        }
        public void Cleanup()
        {
            pairs.Clear();
        }
    }
}
