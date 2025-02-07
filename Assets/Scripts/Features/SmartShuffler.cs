using UnityEngine;
using Random = System.Random;

namespace Features
{
    public class SmartShuffler
    {
        public delegate void ShuffledDelegate();
        public event ShuffledDelegate BoardShuffled;

        private BoardData _boardData;
        private readonly int _rows;
        private readonly int _columns;
        private readonly int _totalTileTypes;
        private readonly int[,] _grid;

        public SmartShuffler( int totalTileTypes, BoardData boardData)
        {
            _boardData = boardData;
            _rows = boardData.Grid().GetLength(0);
            _columns = boardData.Grid().GetLength(1);
            _grid = boardData.Grid();
            _totalTileTypes = totalTileTypes;
            
            _boardData.GridGenerated += OnGridGenerated;
        }

        public void Shuffle() // first, generate a full grid, then shuffle it on OnGridGenerated
        {
            _boardData.GenerateFullGrid();
        }

        private void OnGridGenerated()
        {
            Random random = new Random();
            int preDefinedGroupCount = random.Next(1, 3);
            int preDefinedBlockCount = 2;

            for (int i = 0; i < preDefinedGroupCount; i++)
            {
                int row = random.Next(0, _rows);
                int col = random.Next(0, _columns);
                int tileType = random.Next(0, _totalTileTypes);
                _grid[row, col] = tileType;

                for (int j = 0; j < preDefinedBlockCount; j++)
                {
                    Vector2Int neighbour = GetRandomNeighbour(row, col);
                    _grid[neighbour.y, neighbour.x] = tileType;
                }
            }
            BoardShuffled?.Invoke();
        }
        
        private Vector2Int GetRandomNeighbour(int row, int col)
        {
            Random random = new Random();

            int direction, newRow, newCol;

            do
            {
                direction = random.Next(-1, 1);
                newRow = row + Mathf.Clamp(direction, -1, 1);
                direction = random.Next(-1, 1);
                newCol = col + Mathf.Clamp(direction, -1, 1);

            } while (!IsInBounds(newRow, newCol));

            return new Vector2Int(newCol, newRow);
        }

        private bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < _rows && col >= 0 && col < _columns;
        }
        
        
        ~SmartShuffler()
        {
            _boardData.GridGenerated -= OnGridGenerated;
        }
    }
}