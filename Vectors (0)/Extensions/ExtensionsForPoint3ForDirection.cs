namespace IziHardGames.Libs.NonEngine.Vectors.ExtensionsForDirections
{

	public static class ExtensionsForPoint3ForDirection
	{
		/// <summary>
		/// Clockwise:<br/>
		/// 0 - Right<br/>
		/// 1 - RightTop<br/>
		/// 2 - Top<br/>
		/// 3 - LeftTop<br/>
		/// 4 - Left<br/>
		/// 5 - LeftBot<br/>
		/// 6 - Bot<br/>
		/// 7 - RightBot<br/>
		/// 8 - Center<br/>
		/// </summary>
		/// <returns></returns>
		public static int GetDirectionQuarterXY(ref this Point3 point)
		{
			if (point.x > 0)
			{
				//Right
				if (point.y > 0)
				{
					// Right Top
					return 1;
				}
				else
				{
					if (point.y < 0)
					{
						// Right Bot
						return 7;
					}
					else
					{
						// Right
						return 0;
					}
				}
			}
			else
			{
				if (point.x < 0)
				{
					//Left
					if (point.y > 0)
					{
						//Left Top
						return 3;
					}
					else
					{
						if (point.y < 0)
						{
							// Left Bot
							return 5;
						}
						else
						{
							//Left
							return 4;
						}
					}
				}
				else
				{
					// Bot/Center/Top
					if (point.y > 0)
					{   // Top
						return 2;
					}
					else
					{
						if (point.y < 0)
						{
							// Bot
							return 6;
						}
						else
						{
							// center 
							return 8;
						}
					}
				}
			}
		}
	}
}
