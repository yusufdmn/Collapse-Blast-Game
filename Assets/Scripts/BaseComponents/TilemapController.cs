using UnityEngine;
using UnityEngine.Tilemaps;

namespace BaseComponents
{
    public class TilemapController
    {
        private readonly Tilemap _tilemap;

        public Vector3Int WorldToCell(Vector3 worldPosition) => _tilemap.WorldToCell(worldPosition);


        public TilemapController(Tilemap tilemap)
        {
            _tilemap = tilemap;
        }
        
        public void ClearTile(Vector3Int cellPosition)
        {
            _tilemap.SetTile(cellPosition, null);
        }
        
        public void SetTile(Vector3Int cellPosition, TileBase tile)
        {
            _tilemap.SetTile(cellPosition, tile);
        }
    }
}