using IziHardGames.Ticking.Lib.ApplicationLevel;
using IziHardGames.ProjectResourceManagment;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.GameProject1.Handbooks
{
	public abstract class Handbook<THandbook> : MonoBehaviour
		where THandbook : IziHardGames.Core.IUnique
	{
		public List<THandbook> presets;
		public List<int> ids;
		public static readonly Dictionary<int, THandbook> keyValuePairs = new Dictionary<int, THandbook>();
#if UNITY_EDITOR
		[NonSerialized] public RegistryAsset registryAsset;
#endif

		#region Unity Message
		protected virtual void Reset()
		{

		}
		protected virtual void OnDestroy()
		{
			InitilizeReverse();
		}
		#endregion

		public virtual void InitilizeReverse()
		{
			Reg.SingletonRemove(this);

			keyValuePairs.Clear();
		}
		public virtual void Initilize()
		{
			Reg.SingletonAdd(this);

			foreach (var item in presets)
			{
				keyValuePairs.Add(item.Id, item);
			}
		}

		public virtual void Clear()
		{
			presets.Clear();
			ids.Clear();
			keyValuePairs.Clear();
		}

		public THandbook GetPreset(int id)
		{
			return presets.FirstUnique(id);
		}
		public static bool TryGet(int id, out THandbook handbook)
		{
			return keyValuePairs.TryGetValue(id, out handbook);
		}
		public static THandbook Get(int id)
		{
			return keyValuePairs[id];
		}

		public THandbook GetById(int id)
		{
			return keyValuePairs[id];
		}

		public List<THandbook> GetHandbook()
		{
			return presets;
		}

#if UNITY_EDITOR
		[ContextMenu("Сфомрировать справочные объекты")]
		public void CallGenerate()
		{
			Generate();
		}

		public virtual void Generate()
		{
			Clear();

			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif
	}

	///// <summary>
	///// Форма с повторяющимимчя элементами
	///// </summary>
	///// <typeparam name="TData"></typeparam>
	//[Obsolete("Incomplete")]
	//public abstract class FormList<TData, THandbook, THandbookManager> : FormBase
	//	where THandbookManager : Handbook<THandbook>
	//	where THandbook : IUnique
	//{
	//	[SerializeField] protected Transform parent;
	//	[SerializeField] protected GameObject prefab;

	//	[SerializeField] protected List<RedirectorId> redirectorIds;
	//	protected List<THandbook> datas = new List<THandbook>(100);

	//	protected PoolGameObjects poolGameObjects;

	//	public override void Initilize()
	//	{
	//		base.Initilize();

	//		poolGameObjects = new PoolGameObjects(100, () => Object.Instantiate(prefab, parent), Object.Destroy);
	//	}

	//	public override void UpdateUi()
	//	{
	//		base.UpdateUi();
	//	}
	//	public override void Clean()
	//	{
	//		base.Clean();

	//		redirectorIds.ForEach(x => x.gameObject.SetActive(false));

	//		foreach (var item in redirectorIds)
	//		{
	//			item.Free();

	//			poolGameObjects.Return(item.gameObject);
	//		}

	//		redirectorIds.Clear();

	//		datas.Clear();
	//	}
	//}
}

