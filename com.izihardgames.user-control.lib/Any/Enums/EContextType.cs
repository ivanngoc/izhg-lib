namespace IziHardGames.UserControl.Lib.Contexts
{
	/// <summary>
	/// Predefined types of context
	/// </summary>
	public enum EContextType
	{
		None,
		/// <summary>
		/// Current object under pointer
		/// </summary>
		Pointer,
		/// <summary>
		/// Current active form 
		/// </summary>
		FocusForm,
		/// <summary>
		/// Current active element of form
		/// </summary>
		FocusElement,
		/// <summary>
		/// Object from select action
		/// </summary>
		Selection,
		/// <summary>
		/// Object from last selec action. Can't be null if since start of environment (level and etc) at least once selection were perfomed. 
		/// </summary>
		LastSelection,
	}
}