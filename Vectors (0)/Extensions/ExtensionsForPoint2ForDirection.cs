namespace IziHardGames.Libs.NonEngine.Vectors.ExtensionsForDirections
{
	public static class ExtensionsForPoint2ForDirection
	{

		/// <inheritdoc cref="ExtensionsForPoint3ForDirection.GetDirectionQuarterXY(ref Point3)"/>
		public static int GetDirectionQuarter(this Point2 point)
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
