using IziHardGames.Libs.Engine.Memory;
using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.Engine.Applications
{
	public class UpdatableTimeGroupe : IPoolable
	{
		internal int key;
		internal float timeSinceLastUpdate;
		internal float periodInSeconds;
		internal List<Action> callbacks = new List<Action>(64);

		internal void Add(Action callback)
		{
#if UNITY_EDITOR
			if (callbacks.Contains(callback)) throw new ArgumentException($"{callback.Target.ToString()}.{callback.Method.Name}");
#endif
			callbacks.Add(callback);
		}
		internal void Remove(Action callback)
		{
#if UNITY_EDITOR
			if (callbacks.Contains(callback)) throw new ArgumentException($"{callback.Target.ToString()}.{callback.Method.Name}");
#endif
			callbacks.Add(callback);
		}

		internal void Update(float deltaTime)
		{
			timeSinceLastUpdate += deltaTime;

			if (timeSinceLastUpdate > periodInSeconds)
			{
				timeSinceLastUpdate = default;

				foreach (var callback in callbacks)
				{
					callback.Invoke();
				}
			}
		}


		public void CleanToReuse()
		{
			callbacks.Clear();
			key = default;
			timeSinceLastUpdate = default;
			periodInSeconds = default;
		}

		public void ReturnToPool()
		{
			CleanToReuse();
			PoolObjects<UpdatableTimeGroupe>.Shared.Return(this);
		}
	}
}