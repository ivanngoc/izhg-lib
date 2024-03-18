using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.GameProject1
{
	public class ControlsHubScriptable : MonoBehaviour
	{
		[SerializeField] List<ScriptableObject> controls;

		#region Unity Message


		#endregion

		public T Get<T>() where T : ScriptableObject
		{
			foreach (var item in controls)
			{
				if (item is T) return item as T;
			}

			return default;
		}
	}
}