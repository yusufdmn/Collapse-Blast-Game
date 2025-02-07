using BaseComponents;
using Data;
using UnityEngine;

public class BoardRenderer
{
    public delegate void GridRenderedDelegate();
    public event GridRenderedDelegate GridRendered;
    
    private readonly TilemapController _tilemapController;
    private readonly BlockTilesSO _defaultBlockTilesSo;
    
    public BoardRenderer(BlockTilesSO defaultBlockTilesSo, TilemapController tilemapController)
    {
        _tilemapController = tilemapController;
        _defaultBlockTilesSo = defaultBlockTilesSo;
    }

    public void RenderFullGrid(int[,] grid)
    {
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                int tileType = grid[row, col];
                Vector3Int tilePosition = new Vector3Int(col, row, 0);
        
                if (tileType < _defaultBlockTilesSo.Tiles.Length)
                {
                    _tilemapController.SetTile(tilePosition, _defaultBlockTilesSo.Tiles[tileType]);
                }
                else
                {
                    _tilemapController.SetTile(tilePosition, null);
                }
            }
        }
        GridRendered?.Invoke();
    }

}