using Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockFactory : MonoBehaviour
{
    [SerializeField] private BlockTiles _defaultBlockTiles;
    [SerializeField] private Tilemap _tilemap;
    
    public void RenderFullGrid(BoardData boardData)
    {
        int[,] grid = boardData.GetGrid();
        
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                int tileIndex = grid[row, col];
                Vector3Int tilePosition = new Vector3Int(col, row, 0);
        
                if (tileIndex < _defaultBlockTiles.Tiles.Length)
                {
                    _tilemap.SetTile(tilePosition, _defaultBlockTiles.Tiles[tileIndex]);
                }
                else
                {
                    _tilemap.SetTile(tilePosition, null);
                }
            }
        }
    }


    public void ClearTile(int row, int col)
    {
        Vector3Int tilePosition = new Vector3Int(col, row, 0);
        _tilemap.SetTile(tilePosition, null);
    }

    public void SetTile(int row, int col, int tileIndex)
    {
        Vector3Int tilePosition = new Vector3Int(col, row, 0);
        _tilemap.SetTile(tilePosition, _defaultBlockTiles.Tiles[tileIndex]);
    }
}