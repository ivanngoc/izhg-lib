using System;
using System.Runtime.InteropServices;

namespace IziHardGames.Libs.NonEngine.Vectors
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct Point2Int : IVector2<int>, IEquatable<Point2Int>, IComparable<Point2Int>
	{
		[FieldOffset(0)] public int x;
		[FieldOffset(4)] public int y;
		public int X { get => x; set => x = value; }
		public int Y { get => y; set => y = value; }
		public float Magnitude => throw new NotImplementedException();
		public float SqrMagnitude => throw new NotImplementedException();

		public static readonly Point2Int zero;
		public static readonly Point2Int one;
		public static readonly Point2Int max;
		public static readonly Point2Int min;

		static Point2Int()
		{
			zero = new Point2Int(0, 0);
			one = new Point2Int(1, 1);
			min = new Point2Int(int.MinValue, int.MinValue);
			max = new Point2Int(int.MaxValue, int.MaxValue);
		}

		public Point2Int(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public static implicit operator Point3Int(Point2Int val) => new Point3Int(val.x, val.y, default);
		public static implicit operator Point2Int(Point3Int val) => new Point2Int(val.x, val.y);
		public static Point2Int operator +(Point2Int a, Point2Int b) => new Point2Int(a.x + b.x, a.y + b.y);
		public static Point2Int operator -(Point2Int a, Point2Int b) => new Point2Int(a.x - b.x, a.y - b.y);
		public bool Equals(Point2Int other)
		{
			throw new NotImplementedException();
		}
		public int CompareTo(Point2Int other)
		{
			throw new NotImplementedException();
		}
		public override bool Equals(object obj)
		{
			throw new NotSupportedException();
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(x, y);
		}
	}
}