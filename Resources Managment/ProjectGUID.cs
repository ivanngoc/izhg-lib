using System;

namespace IziHardGames
{
	/// <summary>
	/// Отрицательные id для черновиков и пробников (создается динамически)<br/>
	/// Положительные id для объектов в консервации и для объектов подстановки (востребованные поля)<br/>
	/// </summary>
	[Serializable]
	public struct ProjectGUID : IGuid<int>
	{
		public int GUID { get => guid; set => guid = value; }
		/// <summary>
		/// Global
		/// </summary>
		public int guid;
		/// <summary>
		/// Typical (Per Type)
		/// </summary>
		public int id;

		public int idType;
		/// <summary>
		/// 0-class
		/// 1-exemplar
		/// </summary>
		public int idValueType;
	}

	interface IGuid<T>
	{
		T GUID { get; set; }
	}
}