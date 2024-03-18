using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Scheming.Datas
{
	/// <summary>
	/// Это условное разделение массива по аналогии как на прямой (массиве) создается направленный отрезок с начальной точкой и конечной.
	/// Элементы в каждой группе обязанны идти последовательно и неразрывно в <see cref="DataSchema.items"/>.
	/// С таким подходом можно создавать также и вложенные группы. Но необзодимо ввести проверку чтобы не получилось так что вложенная группа выходила за индекс последнего элемента группы в которую вложенная группа включена.
	/// Иными словами все группы ниже не могут быть дальше любой из группы выше (иметь больший <see cref="indexEnd"/>, но моежт иметь равный)  
	/// </summary>
	[Serializable]
	public class DataSchemaGroupe
	{
		public int id;
		/// <summary>
		/// indexes as ids in <see cref="DataSchema"/>
		/// </summary>
		public List<int> items;

		/// <summary>
		/// Represend index in <see cref="DataSchema.items"/> from which group is starting inclusive
		/// </summary>
		public int indexStart;
		/// <summary>
		///  Represend index in <see cref="DataSchema.items"/> on which groupe ending inclusive
		/// </summary>
		public int indexEnd;

		public int channel;
	}
}