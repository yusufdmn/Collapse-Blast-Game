using UnityEngine;
using UnityEngine.Tilemaps;

namespace Data
{
    [CreateAssetMenu(fileName = "Blocks", menuName = "Settings/Block Tiles")]
    public class BlockTilesSO: ScriptableObject
    {
        public TileBase[] Tiles;
    }
}