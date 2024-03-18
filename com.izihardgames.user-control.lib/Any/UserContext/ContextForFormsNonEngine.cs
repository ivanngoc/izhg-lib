using System;
using IziHardGames.UserControl.Abstractions.NetStd21.Contexts;

namespace IziHardGames.UserControl.Lib
{
	public class ContextForFormsNonEngine : UserContext
	{
		public void FormClose()
		{
			throw new NotImplementedException();
		}
		public void FormOpen()
		{
			throw new NotImplementedException();
		}
		public void WindowOpen()
		{
			throw new NotImplementedException();
		}
		public void WindowClose()
		{
			throw new NotImplementedException();
		}
	}
}