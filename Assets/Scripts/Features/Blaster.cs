using System.Collections.Generic;
using BaseComponents;
using Helper;
using Settings;
using UnityEngine;

namespace Features
{
    public class Blaster
    {
        private readonly TargetGroupFinder _targetGroupFinder = new();
        private readonly TilemapController _tilemapController;
        private readonly int _rows, _cols;
    
        public Blaster(TilemapController tilemapController, LevelSettings levelSettings)
        {
            _rows = levelSettings.Rows;
            _cols = levelSettings.Columns;
            _tilemapController = tilemapController;
        }
        
        
        #region Public API
        public void Blast(List<List<Vector2Int>> allGroups, Vector3 clickWorldPosition)
        {
            Vector3Int cellPosition = _tilemapController.WorldToCell(clickWorldPosition);

            if (!IsInBounds(cellPosition)) // check if the click is within the grid bounds
                return;
        
            // find the group that contains the target block
            Vector2Int targetTile = new Vector2Int(cellPosition.x, cellPosition.y); 
            List<Vector2Int> targetGroup = _targetGroupFinder.GetTargetGroup(allGroups, targetTile);  
        
            // if the target block is not in any group, don't blast anything
            if (targetGroup == null) 
                return;

            // blast the target group from the grid
            foreach (var tile in targetGroup)
            {
                _tilemapController.SetTile(new Vector3Int(tile.x, tile.y, 0), null);
            }
        }
    
        #endregion
        
        #region Private Methods
        
        private bool IsInBounds(Vector3Int cellPosition) // check if the click is within the grid bounds
        {
            int row = cellPosition.y;
            int col = cellPosition.x;

            if (row < 0 || row >= _rows || col < 0 || col >= _cols)
            {
                return false;
            }
            return true;
        }
        
        #endregion
    }
}