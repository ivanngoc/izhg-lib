using UnityEngine;

namespace IziHardGames.Grids.D2.ForUnity
{
    public class InstantiateAtGridCell : MonoBehaviour, IGridCellProcessor
    {
        [SerializeField] private GameObject? prefab;

        public void HandleCell(Vector3 pos, int x, int y, int z)
        {
            var go = GameObject.Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}