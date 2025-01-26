using UnityEngine;
using UnityEngine.Tilemaps;

namespace Settings
{
    [CreateAssetMenu(fileName = "Blocks", menuName = "Settings/Block Tiles")]
    public class BlockTiles: ScriptableObject
    {
        public TileBase[] Tiles;
    }
}