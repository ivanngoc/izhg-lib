namespace IziHardGames.Libs.NonEngine.ConditionalOperation
{
	/// <summary>
	/// ќбработка операций сравнений и эквивалентости с заданынм эталоном
	/// </summary>
	public class ConditionProcessor
	{
		public JobEqual[] jobEquals;

		#region Unity Message

		#endregion
	}

	public interface IConditionJob
	{

	}

	public struct JobEqual : IConditionJob
	{
		/// <summary>
		///  идентфикатор
		/// </summary>
		public int id;

		public bool result;
		public byte value0;
		public byte value1;
		public byte value2;
		/// <summary>
		/// Ёталонное значение
		/// </summary>
		public int value;
		/// <summary>
		/// ¬нешнее значение с которым нужно сравнить
		/// </summary>
		public int valueOut;
	}

	public struct JobLess : IConditionJob
	{
		public int key0;
		public int key1;
		public bool result;
		public int value;
	}
	public struct JobLessOrEqual : IConditionJob
	{
		public int key0;
		public int key1;
		public bool result;
		public int value;
	}
}