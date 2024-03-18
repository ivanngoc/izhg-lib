using UnityEngine;

namespace IziHardGames.Grids.D2.ForUnity
{
    public interface IGridCellProcessor
    {
        public void HandleCell(Vector3 pos, int x, int y, int z);
    }
}