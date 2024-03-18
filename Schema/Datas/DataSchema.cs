using System;

namespace IziHardGames.Libs.NonEngine.Scheming.Datas
{
	/// <summary>
	/// Scheme based on type. 
	/// </summary>
	[Serializable]
	public class DataSchema
	{
		/// <summary>
		/// Baked schema
		/// </summary>
		public DataSchemaItem[] items;
		public DataSchemaGroupe[] groups;
		/// <summary>
		/// Values for parsing. For example if value is type than after export this values used to find <see cref="Type"/> through reflections
		/// </summary>
		public string[] values;

		public DataSchema()
		{
		}

		public int IndexOf(string type)
		{
			throw new NotImplementedException();
		}
	}
}