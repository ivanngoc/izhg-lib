namespace IziHardGames.Libs.NonEngine.View
{
#if UNITY_EDITOR
	public class InterfacesViewNonEngine
	{
		#region Unity Message		

		#endregion
	}
#endif

}


namespace IziHardGames.View
{
	public interface IView
	{

	}
	public interface ISpriteAssignable
	{
		int IdSprite { get; set; }
	}
	public interface IViewable
	{
		public bool IsVisible { get; set; }
		public void Show();
		public void Hide();
	}
	/// <summary>
	/// В отличие от <see cref="IReactiveUi"/> применяется ко всему кроме UI
	/// </summary>

	public interface IViewUpdatable
	{ }

	public interface IViewUpdater { }

	public interface IViewUpdaterPosition : IViewUpdater
	{

	}

	public interface IViewUpdaterMaterialOfRenderer : IViewUpdater
	{

	}
}