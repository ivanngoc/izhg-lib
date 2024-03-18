using System;
using System.Collections.Generic;

namespace IziHardGames.ProjectResourceManagment
{
	[Serializable]
	public class TagAsset
	{
		public int id;
		public string tagDescription;
	}

	[Serializable]
	public class TagAssetGroupe
	{
		public int id;
		public List<int> idsTags;
		public string description;
	}
}