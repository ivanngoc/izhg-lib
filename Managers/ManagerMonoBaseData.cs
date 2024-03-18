using IziHardGames.Core;
using IziHardGames.View;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Managers
{
	/// <summary>
	/// Factory + Manager
	/// </summary>
	/// <typeparam name="TComp"></typeparam>
	/// <typeparam name="TData"></typeparam>
	public class ManagerMonoBaseData<TComp, TData> : ManagerForMonoComponents<TComp>
		where TComp : Component, IUnique, IInitializable, IInitializable<TData>, IDeinitializable, IVisionControllable
		where TData : IUnique
	{
		public new static ManagerMonoBaseData<TComp, TData> singleton;

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
		public override void InitilizeReverse()
		{
			base.InitilizeReverse();
			singleton = default;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public TComp Rent(TData data, bool isVisible = default)
		{
			var gameObject = poolGameObjects.Rent();
			var comp = gameObject.GetComponent<TComp>();
			comp.Initilize(data);
			manager.Add(comp);

			if (isVisible)
			{
				comp.ControlView.Show();
			}

			return comp;
		}
		public TComp Return(TData obj)
		{
			return Return<TComp>(GetExisted(obj.Id), true);
		}
	}
}