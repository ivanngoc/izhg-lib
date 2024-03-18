using System;

namespace IziHardGames.Libs.NonEngine.Rectangles
{
	[Serializable]
	public struct Rect2D : IEquatable<Rect2D>
	{
		// position of origin point
		public float x;
		public float y;

		public float width;
		public float height;

		public Rect2D(float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public bool Equals(Rect2D other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			throw new NotSupportedException();
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(x, y, width, height);
		}
		
		public static bool operator ==(Rect2D a, Rect2D b)
		{
			return a.x == b.x && a.y == b.y && a.width == b.width && a.height == b.height;
		}
		public static bool operator !=(Rect2D a, Rect2D b)
		{
			return a.x != b.x || a.y != b.y || a.width != b.width || a.height != b.height;
		}
	}
}