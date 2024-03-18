using System;
using System.Collections.Generic;


namespace IziHardGames.Libs.NonEngine.GameLogic.CheckPointSystem
{
	public class CheckPointSystem
	{

	}
	/// <summary>
	/// Система продвижения по контррольным точкам.
	/// </summary>
	public class CheckPointForwardingSystem
	{
		public static int indexCurrent;
		public static CheckPoint[] checkPoints;

		public static void Start(CheckPoint[] checkPointsArg, int indexArg)
		{
			checkPoints = checkPointsArg;
			indexCurrent = indexArg;

			for (int i = 0; i < checkPointsArg.Length; i++)
			{
				CheckPointHandlers.SetHandler(i, CheckPointHandlers.actions[i]);
			}
			if (checkPointsArg.Length > 0)
			{
				CheckPointHandlers.RunHandler(indexCurrent);
			}
		}
		public static void NotImpl()
		{
			throw new NotImplementedException();
		}
		public static void Next()
		{
			indexCurrent++;
			CheckPointHandlers.RunHandler(indexCurrent);
		}
	}

	public static class CheckPointHandlers
	{
		public static Action[] actions;

		public static void Initilize(int countHandlers)
		{
			actions = new Action[countHandlers];
		}

		public static void SetHandler(int index, Action handler)
		{
			actions[index] = handler;
		}

		public static void RunHandler(int index)
		{
			actions[index].Invoke();
		}
	}

	/// <summary>
	/// Контрольная точка сюжета или игры. Необходим для вызова триггеров. Набор из объектов чекпоинтов создается заранее и известен игре заранее. 
	/// По этому набору происходит двжиение по сюжету игры или по прогрессу игры. Набор не выгружается в объект сохранения игрока
	/// </summary>
	[Serializable]
	public class CheckPoint
	{
		public int idCheckPoint;
		public int order;
		/// <summary>
		/// Идентификатор триггера (Метода обработчика)
		/// </summary>
		public int idActionEnter;

#if UNITY_EDITOR
		private static int idTemp;
		private static int orderTemp;
		public CheckPoint DoCheckPoint(List<CheckPoint> checkPoints, int idActionP)
		{
			idCheckPoint = idTemp;
			order = orderTemp;
			idActionEnter = idActionP;

			checkPoints.Add(this);

			idTemp++;
			orderTemp++;
			return this;
		}
		public enum ECheckPointAction
		{
			AwaitTutorialAction,
			CreateEvent,
			BeginTutorial,
		}
#endif

	}
	/// <summary>
	/// Конкретное состояние <see cref="CheckPoint"/>. Выгружается в файл сохранения игры
	/// </summary>
	public class DataCheckPoint
	{
		public int idCheckPoint;
		public int state;
	}

	/// <summary>
	/// Данные о текущем положении
	/// </summary>
	public class DataCheckPointProgress
	{
		public int id;
		public int idCheckPointCurrent;
	}
}