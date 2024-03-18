using IziHardGames.Core;
using IziHardGames.View;
using System;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Managers
{
	[Serializable]
	public class ManagerMonoBaseDataNonMono<T, TData> : ManagerMonoBaseNonMono<T>
		where T : Component, IUnique, IInitializable, IInitializable<TData>, IDeinitializable, IVisionControllable
		where TData : IUnique
	{
		public new static ManagerMonoBaseDataNonMono<T, TData> singleton;

		#region Unity Message
		[ContextMenu("Base Reset")]
		public virtual void Reset()
		{
			//if (prefab != null)
			//{
			//    if (prefab.activeSelf)
			//    {
			//        Debug.LogError($"Prefab is Set Active. It may cause perfomance degrodation during pooling", prefab);
			//    }
			//}
		}
		#endregion

		public override void Initilize()
		{
			base.Initilize();

			singleton = this;
		}
		public override void Initilize_De()
		{
			base.Initilize_De();

			singleton = default;
		}
		/// <summary>
		/// <see cref="ManagerMonoBaseNonMono{T}{T}.GetNew(int, bool)"/> - отличается параметром TData
		/// </summary>
		/// <returns></returns>
		public T GetNew(TData data, bool isVisible = default)
		{
			var gameObject = poolGameObjects.Rent();

			var comp = gameObject.GetComponent<T>();

			comp.Initilize(data);

			manager.Add(comp);

			if (isVisible)
			{
				comp.ControlView.Show();
			}

			return comp;
		}
		public T Return(TData obj)
		{
			return Return(GetExisted(obj.Id));
		}
	}
}