using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IziHardGames.ProjectResourceManagment
{
	[CreateAssetMenu(fileName = "TaggedAsset", menuName = "IziHardGames/Tagged Asset", order = 1)]
	public class TaggedAsset : ScriptableObject
	{
		[SerializeField] public Object asset;
		[SerializeField] public int idProjectAsset;
		[SerializeField] public string idAsset;
		[SerializeField] public long localIdentificator;
		[SerializeField] public EAssetType eAssetType;

		[SerializeField] List<TagAsset> tags;

		#region Unity Message


		#endregion
#if UNITY_EDITOR

		public EAssetType DefineType()
		{
			Type type = asset.GetType();

			if ((typeof(Texture)).IsAssignableFrom(type))
			{
				if (IsLoadedAsType<Sprite>(out Sprite sprite))
				{
					eAssetType = EAssetType.Sprite;
				}
				else
				{
					eAssetType = EAssetType.Texture;
				}
			}
			return eAssetType;
		}

		public bool IsLoadedAsType<T>(out T loaded) where T : Object
		{
			loaded = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(idAsset), typeof(T)) as T;

			return loaded != null;
		}

		public Sprite AsSprite()
		{
			IsLoadedAsType(out Sprite sprite);

			return sprite;
		}
#endif

		public enum EAssetType
		{
			None,
			Sprite,
			Texture,
		}
	}
}