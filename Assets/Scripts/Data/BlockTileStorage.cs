using UnityEngine;

namespace Data
{
    public class BlockTileStorage: MonoBehaviour
    {
        [SerializeField] private BlockTilesSO defaultBlockTiles;
        [SerializeField] private BlockTilesSO blockTilesA;
        [SerializeField] private BlockTilesSO blockTilesB;
        [SerializeField] private BlockTilesSO blockTilesC;
    
        public BlockTilesSO DefaultBlockTilesSo => defaultBlockTiles;
        public BlockTilesSO BlockTilesSoA => blockTilesA;
        public BlockTilesSO BlockTilesSoB => blockTilesB;
        public BlockTilesSO BlockTilesSoC => blockTilesC;
        
    }
}