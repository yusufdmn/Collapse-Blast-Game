using Settings;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private BlockFactory blockFactory;
    
    private BoardData _boardData;

    void Start()
    {
        _boardData = new BoardData(_levelSettings); // Initialize grid data
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        _boardData.GenerateFullGrid();
        blockFactory.RenderFullGrid(_boardData);
    }
    

    public void ClearTile(int row, int col)
    {
        _boardData.ClearCell(row, col); // Clear cell in grid data
        blockFactory.ClearTile(row, col); // Clear tile on the Tilemap
    }

    public void SetTile(int row, int col, int tileIndex)
    {
        _boardData.SetCell(row, col, tileIndex); // Update cell in grid data
        blockFactory.SetTile(row, col, tileIndex); // Update tile on the Tilemap
    }
}