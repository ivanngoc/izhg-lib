using IziHardGames.View;

namespace IziHardGames.Libs.Engine.View
{
	public class CounterChangesOfViewWithComplexFlag
	{
		public const int MASK_BIT_OF_CHANGING_POSITION = 1 << 1;
		public const int MASK_BIT_OF_CHANGING_POSITION_REVERSE = ~MASK_BIT_OF_CHANGING_POSITION;
		public const int MASK_BIT_OF_CHANGING_ROTATION = 1 << 2;
		public const int MASK_BIT_OF_CHANGING_SCALE = 1 << 3;
		public const int MASK_BIT_OF_CHANGING_VISIBILITY = 1 << 4;
		public const int MASK_BIT_OF_CHANGING_MATERIAL = 1 << 5;
		public const int MASK_BIT_OF_CHANGING_COLOR = 1 << 6;
		public const int MASK_BIT_OF_CHANGING_SPRITE = 1 << 7;
		public const int MASK_BIT_OF_CHANGING_MESH = 1 << 8;
		public const int MASK_BIT_OF_CHANGING_MAIN_TEXTURE = 1 << 9;

		public IView bound;
		/// <summary>
		/// if 0 - than no changes in this round.
		/// </summary>
		public int value;


		public void ChangeWeight(int value)
		{
			this.value += value;
		}
		public void ApplyFlag(int flag)
		{
			value |= flag;
		}

		public void CommitChangeOfPosition()
		{
			value |= MASK_BIT_OF_CHANGING_POSITION;
		}


		#region Func
		public bool IsChangedPosition()
		{
			return (value & MASK_BIT_OF_CHANGING_POSITION) > 0;
		}
		#endregion
	}
}