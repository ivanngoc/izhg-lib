using IziHardGames.Libs.NonEngine.StateMachines.Datas;

namespace IziHardGames.Libs.NonEngine.StateMachines.Datas
{
	/// В ццентре всего стоят данные  <see cref="IDataOfState"/>. Эти данные изменяются с помощью <see cref="IProcessorOfDataState"/>. Промежуточные результаты не важны. Важно лишь то что лежит в данных.
	/// Одновременно может изменяться сколько угодно состояний одного объекта. 
	/// Для того чтобы ограничить набор состояний изменяемых при определенном условии используется схема <see cref="ISchemaOfStateMachine"/>. Это граф состояний. Ребра графа - переходы, а узлы графа - процессоры состояний.



	/// <inheritdoc cref="IDataOfState"/>
	public interface IDataOfState<TValue> : IDataOfState
	{

	}
	/// <summary>
	/// Обработчик конкретного типа данных состояния
	/// </summary>
	public interface IProcessorOfDataState
	{

	}
	/// <summary>
	/// Объект с данными о выполняемых изменеиях по всем состояниям объекта. 
	/// Например объект содержащий флаги isMoving, isRotateing, isJumping и т.д.
	/// </summary>
	public interface IMonitorOfStateProcessors
	{

	}
	

	/// <summary>
	/// Данные об конкретной машине состояния
	/// </summary>
	public interface IDataOfStateMachine
	{


	}
}