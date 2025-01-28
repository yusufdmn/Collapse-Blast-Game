using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class GroupDetector
    {
        public delegate void GroupDetectedDelegate();

        public event GroupDetectedDelegate GroupsDetected;

        #region Fields

        private readonly int[,] _grid;
        private readonly int _rows, _cols;
        private readonly bool[,] _visited;

        // pre-allocated collections to avoid GC
        private readonly List<Vector2Int> _group = new();
        private readonly List<List<Vector2Int>> _allGroups = new();
        private readonly Queue<List<Vector2Int>> _listPool = new();
        private readonly Stack<Vector2Int> _stackPool = new();

        // Directions to move in the grid
        private readonly int[] rowDirections = { -1, 1, 0, 0 };
        private readonly int[] colDirections = { 0, 0, -1, 1 };

        #endregion

        public List<List<Vector2Int>> AllGroups => _allGroups;

        public GroupDetector(int[,] grid)
        {
            _grid = grid;
            _rows = grid.GetLength(0);
            _cols = grid.GetLength(1);
            _visited = new bool[_rows, _cols];
        }
        
        #region Public API

        public void DetectGroups()
        {
            CleanGroups();

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _cols; col++)
                {
                    if (!_visited[row, col])
                    {
                        DFS(row, col);

                        if (_group.Count >= 2)
                        {
                            List<Vector2Int> newGroup = GetOrCreateList();
                            CopyListNonAlloc(newGroup, _group);
                            _allGroups.Add(newGroup);
                        }

                        _group.Clear();
                    }
                }
            }

            GroupsDetected?.Invoke();
        }
        
        #endregion

        #region Private Methods

        // Clear group content to reuse them
        private void CleanGroups()
        {
            // Return all groups to the pool
            foreach (var group in _allGroups)
            {
                group.Clear();
                _listPool.Enqueue(group);
            }

            _allGroups.Clear();
            Array.Clear(_visited, 0, _visited.Length);
        }

        // Depth First Search to detect the group of the current tile
        private void DFS(int row, int col)
        {
            _stackPool.Clear();

            // Push the first node
            Vector2Int currentTile = new Vector2Int
            {
                x = col,
                y = row
            };

            _stackPool.Push(new Vector2Int(col, row));

            while (_stackPool.Count > 0)
            {
                Vector2Int current = _stackPool.Pop();
                int currentRow = current.y;
                int currentCol = current.x;

                if (_visited[currentRow, currentCol])
                    continue;

                _visited[currentRow, currentCol] = true;
                _group.Add(current);

                for (int i = 0; i < 4; i++)
                {
                    int newRow = currentRow + rowDirections[i];
                    int newCol = currentCol + colDirections[i];

                    if (newRow >= 0 && newRow < _rows &&
                        newCol >= 0 && newCol < _cols &&
                        !_visited[newRow, newCol] &&
                        _grid[newRow, newCol] == _grid[currentRow, currentCol])
                    {
                        currentTile.x = newCol;
                        currentTile.y = newRow;
                        _stackPool.Push(new Vector2Int(newCol, newRow));
                    }
                }
            }
        }

        private List<Vector2Int> GetOrCreateList()
        {
            return _listPool.Count > 0 ? _listPool.Dequeue() : new List<Vector2Int>();
        }

        private void CopyListNonAlloc<T>(List<T> target, List<T> source)
        {
            for (int i = 0; i < source.Count; i++)
            {
                target.Add(source[i]);
            }

        }
        
        
        #endregion
        
        
    }
}