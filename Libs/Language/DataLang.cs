using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Region.Language
{
	[CreateAssetMenu(fileName = "Lang", menuName = "IziHardGames/Localization/Lang")]
	public class DataLang : ScriptableObject
	{
		public int count;
		[DrawInt] public int count2;
		[DrawInt]
		[SerializeField]
		public List<int> lsitint;
		public List<Vector3Int> listVector3;
		//public int count3;
		//[DrawStringHexAttribute]
		[SerializeField] public List<string> strings;

		private int debug;
	}
}