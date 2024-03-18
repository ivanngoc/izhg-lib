using IziHardGames.Libs.NonEngine.Vectors;
using System;
using System.Runtime.InteropServices;

namespace IziHardGames.Libs.NonEngine.Rectangles
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	public struct QuadByCorners : IEquatable<QuadByCorners>
	{
		[FieldOffset(0)] public Point2 leftBot;
		[FieldOffset(8)] public Point2 leftTop;
		[FieldOffset(16)] public Point2 rightBot;
		[FieldOffset(24)] public Point2 rightTop;

		public bool Equals(QuadByCorners other)
		{
			return this.leftBot == other.leftBot && this.leftTop == other.leftTop && this.rightBot == other.rightBot && this.rightTop == other.rightTop;
		}
		public override bool Equals(object obj)
		{
			throw new NotSupportedException();
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(leftBot, leftTop, rightBot, rightTop);
		}

		public static bool operator !=(QuadByCorners lhs, QuadByCorners rhs) => !lhs.Equals(rhs);
		public static bool operator ==(QuadByCorners lhs, QuadByCorners rhs) => lhs.Equals(rhs);
	}
}