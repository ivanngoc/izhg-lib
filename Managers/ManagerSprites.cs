using IziHardGames.Core;
using IziHardGames.Ticking.Lib.ApplicationLevel;
using IziHardGames.ProjectResourceManagment;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Managers
{
	public class ManagerSprites : MonoBehaviour, IInitializable, IDeinitializable
	{
		public static ManagerSprites singleton;

		[SerializeField] private List<int> ids;
		[SerializeField] private List<string> guids;
		[SerializeField] private List<Sprite> sprites;

		private readonly Dictionary<int, Sprite> spritesById = new Dictionary<int, Sprite>();
		private readonly Dictionary<string, Sprite> spritesByGUID = new Dictionary<string, Sprite>();

		[SerializeField] public RegistryAsset registryAsset;

		#region Unity Message	

		private void OnDestroy()
		{
			InitilizeReverse();
		}
		#endregion

		public void InitilizeReverse()
		{
			Reg.SingletonRemove(this);
			spritesById.Clear();
			spritesByGUID.Clear();
		}
		public void Initilize()
		{
			Reg.SingletonAdd(this);
			singleton = this;

			for (int i = 0; i < sprites.Count; i++)
			{
				spritesById.Add(ids[i], sprites[i]);
				spritesByGUID.Add(guids[i], sprites[i]);
			}
		}
		public static Sprite GetGlobal(string idAsset)
		{
			return singleton.Get(idAsset);
		}
		public static Sprite GetGlobal(int id)
		{
			return singleton.Get(id);
		}
		public Sprite Get(int id)
		{
#if UNITY_EDITOR
			if (!spritesById.ContainsKey(id))
			{
				throw new KeyNotFoundException($"Не найден спрайт с id {id}");
			}
#endif
			return spritesById[id];
		}
		public Sprite Get(string guid)
		{
			return spritesByGUID[guid];
		}

		public bool TryGet(int id, out Sprite sprite)
		{
			return spritesById.TryGetValue(id, out sprite);
		}
		public bool TryGet(string guid, out Sprite sprite)
		{
			return spritesByGUID.TryGetValue(guid, out sprite);
		}

#if UNITY_EDITOR
		[ContextMenu("Сгенерировать пресет")]
		public void Import()
		{
			Import(registryAsset);
		}
		public void Import(RegistryAsset registryAsset)
		{
			ids.Clear();
			guids.Clear();
			sprites.Clear();
			spritesById.Clear();
			spritesByGUID.Clear();

			foreach (var item in registryAsset.sprites)
			{
				var indexOf = registryAsset.GetIndex(item.texture);
				var id = registryAsset.ids[indexOf];
				ids.Add(id);
				guids.Add(registryAsset.guids[indexOf]);
				sprites.Add(registryAsset.sprites[indexOf]);
			}
			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif


#if UNITY_EDITOR
		[ContextMenu("Сгенерировать список id спрайтов")]
		public void GenerateIdsFromAssetIds()
		{
			ids.Clear();

			for (var i = 0; i < sprites.Count; i++)
			{
				if (UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(sprites[i], out var guid, out long localId))
				{
					var id = (int)localId;

					ids.Add(id);
				}
				else
				{
					Debug.LogError($"Cant Get guid for Sprite {sprites[i].name}", sprites[i]);
				}
			}
		}
#endif
	}
}