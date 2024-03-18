using IziHardGames.Core;
using System;

namespace IziHardGames
{
	public class NonEngineCoreAttributes
	{

	}

	/// <summary>
	/// <see cref="IInitializable"/>. Исключает тип из сбора ссылок с этим интерфейсом. Например в EnterencSceneControl в методе валидации
	/// </summary>
	public class ExcludeAutoInitilizeAttribute : System.Attribute
	{


	}

	public class IncompleteAttribute : System.Attribute
	{

	}
	public class UnusedAttribute : System.Attribute
	{

	}
	/// <summary>
	/// Изменения происходят также в объекте сохранения или объекте выгрузки данных
	/// </summary>
	public class DataSyncAttribute : Attribute
	{

	}
	/// <summary>
	/// Изменения НЕ ПРОИСХОДЯТ также в объекте сохранения или объекте выгрузки данных
	/// </summary>
	public class NonDataSyncAttribute : Attribute
	{

	}
}

namespace IziHardGames.Develop.Attributes
{
	/// <summary>
	/// Объект создается как справочный
	/// </summary>
	public class HandbookAttribute : Attribute
	{

	}
}
