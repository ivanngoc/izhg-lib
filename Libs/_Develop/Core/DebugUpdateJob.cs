#if UNITY_EDITOR
using IziHardGames.Ticking.Lib;
using UnityEngine;

namespace IziHardGames.Develop.Core
{
	public class DebugUpdateJob : MonoBehaviour
	{
		internal UpdateJob job;
		public int id;
		public static int count;
		public bool isUpdating;

		private void Awake()
		{
			count++;
			id = count;
		}

		private void LateUpdate()
		{
			if (isUpdating != job.updateToken.isUpdating)
			{
				//не работает так как после выключения объекта update не работает
				isUpdating = job.updateToken.isUpdating;
				gameObject.SetActive(isUpdating);
			}
		}
	}
}
#endif
