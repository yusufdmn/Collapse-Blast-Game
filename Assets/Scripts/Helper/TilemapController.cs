using Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BaseComponents
{
    public class TilemapController
    {
        private readonly Tilemap _tilemap;
        private BlockTileStorageSO _blockTileStorageSo;
        private readonly BoardData _boardData;
        
        public TilemapController(Tilemap tilemap, BoardData boardData, BlockTileStorageSO blockTileStorageSo)
        {
            _tilemap = tilemap;
            _boardData = boardData;
            _blockTileStorageSo = blockTileStorageSo;
        }
        

        public Vector3Int WorldToCell(Vector3 worldPosition) => _tilemap.WorldToCell(worldPosition);
        public Vector3 GetCellCenterWorld(Vector3Int cellPosition) => _tilemap.GetCellCenterWorld(cellPosition);
        public TileBase GetTile(Vector2Int cellPosition) => _tilemap.GetTile((Vector3Int)cellPosition);

        public void SetTile(Vector3Int cellPosition, TileBase tile)
        {
            _tilemap.SetTile(cellPosition, tile);
             var blockType = _blockTileStorageSo.GetBlockType(tile);  // set the block type in the board data 
            _boardData.SetCell(cellPosition.y, cellPosition.x, blockType);
        }
        
        public void ClearTile(Vector2Int cellPosition)
        {
            _tilemap.SetTile((Vector3Int)cellPosition, null);
            _boardData.ClearCell(cellPosition.y, cellPosition.x);
        }
        
        public void ClearTile(Vector3Int cellPosition)
        {
            _tilemap.SetTile(cellPosition, null);
            _boardData.ClearCell(cellPosition.y, cellPosition.x);
        }
        


    }
}