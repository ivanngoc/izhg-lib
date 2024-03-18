using IziHardGames.Libs.NonEngine.Graphs.QuadTile.Ground;
using IziHardGames.Libs.NonEngine.Vectors;
using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Graphs.QuadTile
{
	public class HelpFuncsQuadTIle
	{
		public bool TryFindBreak(IEnumerable<Point3Int> vector3IntSurs, Point3Int center)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Поиск замкнутых контуров из набора коориднат. Связи между каждым элементом может быть только по граничущим сторонам, связи по диагонали исключены.
		/// Каждый контур набор координат из предостапвленного массива координат при условии что каждый из них непосредственно граничит хотя бы с одной другой координатой из этого контура.
		/// Минималььный размер конутра - 1 координата, при условии что у нее не будет ни одного соседней координаты из предоставленного массива координат.
		/// МАксимальное количество связей у одной координаты - 4, по числу сторон.
		/// </summary>
		/// <returns></returns>
		public bool TryFindIsolatedСircuits(Point3Int[] positions, out Point3Int[][] results)
		{
			JobFindCircuits jobFindCircuits = new JobFindCircuits();
			jobFindCircuits.Initilize(positions);
			jobFindCircuits.Execute();

			throw new NotImplementedException();
		}
	}	
}//namespace
