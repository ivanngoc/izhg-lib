using System;

namespace IziHardGames.NamespaceWrappers
{
	public class InterfacesUnsorted
	{


	}


}
namespace IziHardGames.Core
{
	public interface IStateResetable
	{
		public void StateReset();
	}
	/// <summary>
	/// Возращает объект к определенному состоянию
	/// </summary>
	public interface IStateResetable<TDefaultState>
	{
		public TDefaultState StateReset();
	}
	/// <summary>
	/// Очищает данные до default
	/// </summary>
	public interface IDataResetable
	{
		public void DataReset();
	}

	public interface IAsignableWithIndex : IAsignable
	{
		public int Index { get; set; }
		public void Asign<T>(T data, int index) where T : IUnique;
	}
	/// <summary>
	/// Можно привязать объект 
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	public interface IAsignable
	{
		public object Bind { get; set; }
		public void Asign<T>(T data) where T : class;
		public void Free();
		public T GetBindAs<T>() where T : class;
	}
	public interface ISingle<T> : ISingle
	{
		T Singleton { get; }
	}

	/// <summary>
	/// Во время работы программы может быть только 1 экземляр объекта или объект является статическим
	/// </summary>
	public interface ISingle
	{

	}
	/// <summary>
	/// Объект имеет идентификатор. В пределах одного типа может быть несколько экземлпяров
	/// </summary>
	public interface IUnique
	{
		int Id { get; set; }
	}


	public interface IOrderable
	{
		public int Priority { get; }
	}

	public interface IContinuationState
	{
		bool IsReady { get; }
	}
	
	public interface IDeinitializable
	{
		void InitilizeReverse();
	}
	/// <summary>
	/// Помечает тип как имеющий основной метод, выполняющий главную функцию, с названием Execute. Аналогично функции Main()
	/// </summary>
	public interface IExecutable
	{
		void Execute();
	}
	public interface ITriggerable
	{
		void Trigger();
	}
	public interface IExecutableFuncBool<T1>
	{
		bool Execute(T1 arg);
	}

	public interface IExecutableFunc<T1, T2>
	{
		T2 Execute(T1 arg);
	}


	public interface IExecutable<T>
	{
		void Execute(T arg);
	}




	


	public interface IExcluded
	{

	}

	public interface IGUID
	{
		public int Guid { get; set; }
	}

	public interface IFuncBool
	{
		Func<bool> Execute();
	}

	public interface IFuncBool<T1>
	{
		Func<T1, bool> Execute();
	}
	public interface IFuncBool<T1, T2>
	{
		Func<T1, T2, bool> Execute();
	}
	public interface IFuncBool<T1, T2, T3>
	{
		Func<T1, T2, T3, bool> Execute();
	}
}