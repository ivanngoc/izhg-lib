using System;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Memory
{
	[Serializable]
	public class PoolGameObjects : PoolObjects<GameObject>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="capacity"><see cref="List{T}.Capacity"/> </param>
		/// <param name="instantiate"></param>
		/// <param name="destroy"></param>
		public PoolGameObjects(int capacity, Func<GameObject> instantiate, Action<GameObject> destroy) : base(capacity, instantiate, destroy)
		{

		}
	}
}
