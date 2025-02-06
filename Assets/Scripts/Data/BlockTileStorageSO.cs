using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Data
{
    [CreateAssetMenu(fileName = "BlockTileStorage", menuName = "Data/BlockTileStorage")]
    public class BlockTileStorageSO: ScriptableObject
    {
        [SerializeField] private BlockTilesSO defaultBlockTiles;
        [SerializeField] private BlockTilesSO blockTilesA;
        [SerializeField] private BlockTilesSO blockTilesB;
        [SerializeField] private BlockTilesSO blockTilesC;
        
        private Dictionary<TileBase, int> _tileToIndexMap;
    
        public BlockTilesSO DefaultBlockTilesSo => defaultBlockTiles;
        public BlockTilesSO BlockTilesSoA => blockTilesA;
        public BlockTilesSO BlockTilesSoB => blockTilesB;
        public BlockTilesSO BlockTilesSoC => blockTilesC;
        
        
        private void OnEnable()
        {
            InitializeTileIndexMap();
        }
        
        public int GetBlockType(TileBase tile)
        {
            return _tileToIndexMap.GetValueOrDefault(tile, -1);
        }

        
        private void InitializeTileIndexMap()
        {
            _tileToIndexMap = new Dictionary<TileBase, int>();

            AddTilesToDictionary(defaultBlockTiles);
            AddTilesToDictionary(blockTilesA);
            AddTilesToDictionary(blockTilesB);
            AddTilesToDictionary(blockTilesC);
        }

        private void AddTilesToDictionary(BlockTilesSO blockTilesSO)
        {
            for (int i = 0; i < blockTilesSO.Tiles.Length; i++)
            {
                _tileToIndexMap[blockTilesSO.Tiles[i]] = i;
            }
        }
        
        
    }
}