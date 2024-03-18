using System;

namespace IziHardGames.Libs.NonEngine.Storages.Multifunc
{
	/// <summary>
	/// Object for creating specific type exemplar with arguments
	/// </summary>
	public interface IFactory
	{

	}
	/// <summary>
	/// Хранилище объектов.
	/// Предоставляет API для создания объектов.
	/// Ключевая особенность - хранение объектов чанками и групповые/индивидуальные операции над объектами. 
	/// Встроенный пул и управление памятью. Разработчик лишь запрашивает объект и оперирует им (изменяет обновляет).
	/// Принцип: глобальный доступ из любого места к объекту
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Store<T>
	{
		private static ref T Build(IFactory factory)
		{
			throw new NotImplementedException();
		}
		private static ref T Build<TArg1>(IFactory factory, TArg1 arg1)
		{
			throw new NotImplementedException();
		}
		private static ref T Allocate()
		{
			throw new NotImplementedException();
		}
		private static ref T Get(int id)
		{
			throw new NotImplementedException();
		}


#if UNITY_EDITOR || DEBUG
		private struct SomeStruct
		{

		}
		public static void TestAPI()
		{
			ref SomeStruct refstr = ref Store<SomeStruct>.Allocate();
			int id = default;
			ref SomeStruct refstrGet = ref Store<SomeStruct>.Get(id);
		}
#endif
	}
}
