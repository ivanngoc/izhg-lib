using System;
using System.Collections.Generic;
using System.Threading;

namespace IziHardGames.Libs.NonEngine.Threading
{
	/// <summary>
	/// “јк как yeild потребл¤ет пам¤ть то нужен объект который будет переиспользоватьс¤ и сохран¤ть состо¤ние замен¤¤ сохранение места остоновки работы
	/// ” каждого треда будет свой экземпл¤р прив¤занный к потоку чтобы не было конкуренции и нежданчика
	/// </summary>
	[Obsolete("Incomplete")]
	public class PerCoreItterator<T>
	{
		public int idThread;
		public Thread thread;

		public Queue<Action> actions;
	}
}