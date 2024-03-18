using IziHardGames.Core;
using System.Linq;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Core
{
	public class IUniqueComponent : MonoBehaviour, IUnique
	{
		[SerializeField] public int id;
		[SerializeField] MonoBehaviour reference;
		public int Id { get => id; set => id = value; }

		#region MyRegion
#if UNITY_EDITOR
		[ContextMenu("Call Reset")]
		private void Reset()
		{
			reference = GetComponents<MonoBehaviour>().FirstOrDefault(x => x is IUniqueRef);
		}
#endif
		#endregion

	}
}