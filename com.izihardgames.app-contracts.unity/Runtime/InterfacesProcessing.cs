namespace IziHardGames.NamespaceWrappers
{
	public class InterfacesProcessing
	{


	}
}

namespace IziHardGames.Core
{
	/// <summary>
	/// Если процесс можно прервать например во время DragAndDrop нажав ESC
	/// </summary>
	public interface IBreakable
	{
		public void Break();
	}
	/// <summary>
	/// Отмена операции например в случае ошибки. 
	/// В отличие от <see cref="IBreakable"/> также информирует пользователя об отмене в виде окна ошибки или уведомлением
	/// </summary>
	public interface ICancelable
	{
		public void Cancel();
	}
	/// <summary>
	/// Приостановка / возобнолвение процесса
	/// </summary>
	public interface ISuspendable
	{
		public void Suspend();
		public void Resume();
	}
	/// <summary>
	/// Ctrz + z
	/// </summary>
	public interface IRevertable
	{
		/// <summary>
		/// Сколько раз можно сделать реверт
		/// </summary>
		public int Depth { get; set; }
		public void Revert();
	}
	/// <summary>
	/// Захват объекта
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICapturable<T>
	{
		public void Capture(T target);
		public void Release(T target);
	}

	public interface IPreviewable
	{
		public void PreviewShow();
		public void PreviewHide();
	}

}
