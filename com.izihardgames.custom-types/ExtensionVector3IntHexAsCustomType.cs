using IziHardGames.Extensions.PremetiveTypes;
using IziHardGames.Tile.Hex.Types;
using System;


namespace UnityEngine
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public static class ExtensionVector3IntHexAsCustomTypes
    {
        public static EHexTileDirectionClockWise GetDirectionNeigbourEnum(this Vector3Int from, Vector3Int to)
        {
            var dir = to - from;

            var isYEven = from.y.IsEven();

            if (dir == Vector3Int.right) return EHexTileDirectionClockWise.North;
            if (dir == Vector3Int.left) return EHexTileDirectionClockWise.South;

            if (isYEven)
            {
                if (dir == Vector3Int.up) return EHexTileDirectionClockWise.NorthWest;
                if (dir == Vector3Int.down) return EHexTileDirectionClockWise.NorthEast;
                if (dir == new Vector3Int(-1, 1, 0)) return EHexTileDirectionClockWise.SouthWest;
                if (dir == new Vector3Int(-1, -1, 0)) return EHexTileDirectionClockWise.SouthEast;
            }
            else
            {
                if (dir == new Vector3Int(1, 1, 0)) return EHexTileDirectionClockWise.NorthWest;
                if (dir == new Vector3Int(1, -1, 0)) return EHexTileDirectionClockWise.NorthEast;
                if (dir == Vector3Int.up) return EHexTileDirectionClockWise.SouthWest;
                if (dir == Vector3Int.down) return EHexTileDirectionClockWise.SouthEast;
            }
            throw new ArgumentException($"Wron method execution. Wrong parameter to. Mast be side-by-side cell");
        }
        public static int GetDirectionClockWiseRoseWind6(this Vector3Int self, ref Vector3Int headTo, EHexTileDirectionClockWise[] include)
        {
            var dir = self.GetHeadingCell(ref headTo);
            // поиск совпадения по часовой стрелке до тех пор пока не попаед во в include
            for (var i = 0; i < 8; i++)
            {
                if (TileHexPack.ECompare(include, dir))
                {
                    if (self.y.IsEven())
                    {
                        switch (dir)
                        {
                            case EHexTileDirectionClockWise.North: return 0;
                            case EHexTileDirectionClockWise.NorthEast: return 1;
                            case EHexTileDirectionClockWise.East: return 1;    //
                            case EHexTileDirectionClockWise.SouthEast: return 2;
                            case EHexTileDirectionClockWise.South: return 3;
                            case EHexTileDirectionClockWise.SouthWest: return 4;
                            case EHexTileDirectionClockWise.West: return 5;    //
                            case EHexTileDirectionClockWise.NorthWest: return 5;
                            default: break;
                        }
                    }
                    else
                    {
                        switch (dir)
                        {
                            case EHexTileDirectionClockWise.North: return 0;
                            case EHexTileDirectionClockWise.NorthEast: return 1;
                            case EHexTileDirectionClockWise.East: return 2;    //
                            case EHexTileDirectionClockWise.SouthEast: return 2;
                            case EHexTileDirectionClockWise.South: return 3;
                            case EHexTileDirectionClockWise.SouthWest: return 4;
                            case EHexTileDirectionClockWise.West: return 4;    //
                            case EHexTileDirectionClockWise.NorthWest: return 5;
                            default: break;
                        }
                    }
                }
                dir++;

                if (dir > EHexTileDirectionClockWise.NorthWest)
                    dir = EHexTileDirectionClockWise.North;
            }
            throw new NotImplementedException("Unexpected value");
        }
        public static EHexTileDirectionClockWise GetHeadingCell(this Vector3Int self, ref Vector3Int headTo)
        {
            var dir = headTo - self;

            if (dir.y == 0)
            {
                if (dir.x > 0) return EHexTileDirectionClockWise.North;
                if (dir.x < 0) return EHexTileDirectionClockWise.South;
            }

            if (dir.x > 0 && dir.y < 0) return EHexTileDirectionClockWise.NorthEast;
            if (dir.x < 0 && dir.y < 0) return EHexTileDirectionClockWise.SouthEast;

            if (dir.x < 0 && dir.y > 0) return EHexTileDirectionClockWise.SouthWest;
            if (dir.x > 0 && dir.y > 0) return EHexTileDirectionClockWise.NorthWest;

            if (dir.x == 0)
            {
                if (dir.y > 0) return EHexTileDirectionClockWise.West;
                if (dir.y < 0) return EHexTileDirectionClockWise.East;
            }
            // case 0,0,0
            throw new NotImplementedException($"Unexpected situation {dir}");
        }
        public static int GetDirectionClockWiseRoseWind6(this Vector3Int self, ref Vector3Int headTo)
        {
            var dir = self.GetHeadingCell(ref headTo);

            if (self.y.IsEven())
            {
                switch (dir)
                {
                    case EHexTileDirectionClockWise.North: return 0;
                    case EHexTileDirectionClockWise.NorthEast: return 1;
                    case EHexTileDirectionClockWise.East: return 1;    //
                    case EHexTileDirectionClockWise.SouthEast: return 2;
                    case EHexTileDirectionClockWise.South: return 3;
                    case EHexTileDirectionClockWise.SouthWest: return 4;
                    case EHexTileDirectionClockWise.West: return 5;    //
                    case EHexTileDirectionClockWise.NorthWest: return 5;
                    default: break;
                }
            }
            else
            {
                switch (dir)
                {
                    case EHexTileDirectionClockWise.North: return 0;
                    case EHexTileDirectionClockWise.NorthEast: return 1;
                    case EHexTileDirectionClockWise.East: return 2;    //
                    case EHexTileDirectionClockWise.SouthEast: return 2;
                    case EHexTileDirectionClockWise.South: return 3;
                    case EHexTileDirectionClockWise.SouthWest: return 4;
                    case EHexTileDirectionClockWise.West: return 4;    //
                    case EHexTileDirectionClockWise.NorthWest: return 5;
                    default: break;
                }
            }
            throw new NotImplementedException("Unexpected value");
        }
        public static TileHexPack GetNeigbours(this Vector3Int self)
        {
            return new TileHexPack()
            {
                center = self,
                n = self.GetNorth(),
                s = self.GetSouth(),
                nw = self.GetNorthWest(),
                ne = self.GetNorthEast(),
                sw = self.GetSouthWest(),
                se = self.GetSouthEast(),
            };
        }
    }
}