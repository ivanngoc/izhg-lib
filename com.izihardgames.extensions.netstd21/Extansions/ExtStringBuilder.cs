using System.Text;

namespace IziHardGames.Extensions.Systems
{
	public static class ExtStringBuilder
	{
		public static StringBuilder Reverse(this StringBuilder self)
		{
			int count = self.Length / 2;

			for (int i = 0; i < count; i++)
			{
				char temp = self[i];

				int inverseIndex = self.Length - 1 - i;

				self[i] = self[inverseIndex];

				self[inverseIndex] = temp;
			}

			return self;
		}
	}
}