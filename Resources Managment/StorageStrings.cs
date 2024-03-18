using IziHardGames.Core;
using System;
using System.Linq;
using UnityEngine;

namespace IziHardGames.Storages
{
	public class StorageStrings : MonoBehaviour, IziHardGames.Core.IInitializable, IDeinitializable
	{
		public static StorageStrings singleton;
		//public static readonly Dictionary<int, string> titles = new Dictionary<int, string>();
		//public static readonly Dictionary<int, string> namesSkills = new Dictionary<int, string>();
		//public static readonly Dictionary<int, string> namesEffects = new Dictionary<int, string>();

		[SerializeField] TextAsset textAsset;
		[SerializeField] public string[] strings;
		[SerializeField] public int[] ids;
		[SerializeField] public string[] content;

		public const string TEMP_STRING = "TEMPORALY_STRING_SET";

		#region Unity Message
		private void OnDestroy()
		{
			IziHardGames.Ticking.Lib.ApplicationLevel.Reg.SingletonRemove(this);
		}
		#endregion

		public void Initilize()
		{
			singleton = this;

			IziHardGames.Ticking.Lib.ApplicationLevel.Reg.SingletonAdd(this);
		}

		public void InitilizeReverse()
		{
			singleton = default;
			IziHardGames.Ticking.Lib.ApplicationLevel.Reg.SingletonRemove(this);
		}

		public static string GetTitle(int id)
		{
			return singleton.content[id];
		}
#if UNITY_EDITOR
		[ContextMenu("Сгенерировать из файла")]
		public void InitFromFile()
		{
			var text = textAsset.text;

			strings = text.Split('\n');

			strings = strings.Where(x => !x.Contains("null")).Where(x => !string.IsNullOrEmpty(x)).ToArray();

			ids = new int[strings.Length];
			content = new string[strings.Length];

			for (var i = 0; i < strings.Length; i++)
			{
				var split = strings[i].Split('\t');

				ids[i] = Convert.ToInt32(split[0], 16);
				content[i] = split[1];
			}
		}
		public static string EditotOnlyGetString(int id)
		{
			try
			{
				return FindObjectOfType<StorageStrings>().content[id];
			}
			catch
			{
				Debug.LogError($"Не получилось взять строку с id {id}", FindObjectOfType<StorageStrings>());

				return $"ERROR string_ID {id}";
			}
		}
#endif

	}
}

namespace IziHardGames.GameProject1.Data
{
	[Serializable]
	public class InfoText : IziHardGames.Core.IUnique, IGUID
	{
		public int Guid { get; set; }

		public int guid;
		public int Id { get => id; set => id = value; }

		public int id;
		/// <summary>
		/// <see cref="ETextType"/>
		/// </summary>
		public int idTextType;

		public string content;

		public int idLanguage;

		public enum ETextType
		{
			/// <summary>
			/// Название чего либо. Короткое
			/// </summary>
			Title,
			/// <summary>
			/// Сообщение для пользователя. Например текст модального сообщения
			/// </summary>
			Message,
			/// <summary>
			/// Текст Опции на выбор
			/// </summary>
			Option,
			/// <summary>
			/// Игровой диалог между персонажами
			/// </summary>
			Dialogue,
			/// <summary>
			/// Очень длинны текст лора рассказывающий историю
			/// </summary>
			Lore,
		}

		public enum ELanguage
		{
			English,
			Russian,
		}


	}

	public class StringTemplate
	{
		private int IDPLACE;
		private int TEXTTYPEPLACE;
		private int LANGUAGEPLACE;
		private string CONTENTTPLACE;

		public InfoText CreationTemplate()
		{
			var infoText = new InfoText()
			{
				id = IDPLACE,
				content = CONTENTTPLACE,
				idLanguage = LANGUAGEPLACE,
				idTextType = TEXTTYPEPLACE,
				guid = default,
			};

			return infoText;
		}
	}

}