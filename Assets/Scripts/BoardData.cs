using Settings;
using UnityEngine;

public class BoardData
{
    private readonly int[,] _grid; // to store the grid state
    private readonly int _rows;
    private readonly int _columns;
    private readonly int _totalTileTypes;

    public BoardData(LevelSettings levelSettings)
    {
        _rows = levelSettings.Rows;
        _columns = levelSettings.Columns;
        _totalTileTypes = levelSettings.TotalTileTypes;
        _grid = new int[_rows, _columns];
    }

    public void GenerateFullGrid()
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                _grid[row, col] = Random.Range(0, _totalTileTypes);
            }
        }
    }

    public void ClearCell(int row, int col)
    {
        _grid[row, col] = -1; // -1 to indicate an empty cell
    }

    public void SetCell(int row, int col, int tileIndex)
    {
        _grid[row, col] = tileIndex;
    }

    public int GetCell(int row, int col)
    {
        return _grid[row, col];
    }

    public int[,] GetGrid()
    {
        return _grid;
    }
}