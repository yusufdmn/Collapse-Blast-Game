using System.Collections.Generic;
using BaseComponents;
using Data;
using Helper;
using Settings;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Features
{
    public class Collapser
    {
        public delegate void CollapseEndedDelegate();
        public event CollapseEndedDelegate CollapseEnded;
        
        #region fields
        private readonly BlockAnimator _blockAnimator;
        private readonly TilemapController _tilemapController;
        private readonly TargetGroupFinder _targetGroupFinder = new();
        private readonly BlockTileStorageSO _blockTileStorageSo;
        private readonly TileBase[] _defaultBlockTiles;
        
        private readonly Dictionary<Vector2Int, int> _blocksToCollapse = new(); // key: block position, value: how many rows to collapse 
        private readonly Dictionary<Vector2Int, int> _newBlocksToCollapse = new(); // key: block position, value: how many rows to collapse
        private readonly Dictionary<int, int> _newBlocksInCols = new(); // key: column index, value: number of new blocks in the column
        private readonly int _newBlockOffset = 10; // row offset for new blocks
        private readonly int rows;
        private readonly int _totalBlockTypes;
        #endregion
        
        public Collapser(TilemapController tilemapController, LevelSettings levelSettings, BlockAnimator blockAnimator, BlockTileStorageSO blockTileStorageSo)
        {
            rows = levelSettings.Rows;
            _tilemapController = tilemapController;
            _blockAnimator = blockAnimator;
            _blockTileStorageSo = blockTileStorageSo;
            _defaultBlockTiles = blockTileStorageSo.DefaultBlockTilesSo.Tiles;
            _totalBlockTypes = levelSettings.TotalTileTypes;
        }


        #region Public API
        public void FlagTilesToCollapse(List<List<Vector2Int>> allGroups, Vector3 clickWorldPosition)
        {
            Vector3Int cellPosition = _tilemapController.WorldToCell(clickWorldPosition);

            // find the group that contains the target block
            Vector2Int targetTile = new Vector2Int(cellPosition.x, cellPosition.y);
            List<Vector2Int> targetGroup = _targetGroupFinder.GetTargetGroup(allGroups, targetTile);

            if (targetGroup == null)
                return;

            foreach (var tile in targetGroup)
            {
                int col = tile.x;
                int row = tile.y;

                // add each upper tile in the column to the collapse list
                for (int i = row + 1; i < rows; i++)
                {
                    Vector2Int upperTile = new Vector2Int(col, i);

                    if (!_blocksToCollapse.TryAdd(upperTile, 1))
                        _blocksToCollapse[upperTile]++;
                }

                // count the new blocks in the column
                if (!_newBlocksInCols.TryAdd(col, 1))
                    _newBlocksInCols[col]++;
                
            }

            foreach (var tile in targetGroup)
            {
                // if there is any tile in target group and also in the collapse list, remove it from collapse list
                if (_blocksToCollapse.ContainsKey(tile))
                    _blocksToCollapse.Remove(tile);
            }

            // add new blocks to the collapse list
            foreach (var column in _newBlocksInCols)
            {
                for (int i = 0; i < column.Value; i++)
                {
                    Vector2Int newBlockPosition = new Vector2Int(column.Key, rows + _newBlockOffset + i);
                    _newBlocksToCollapse.TryAdd(newBlockPosition, _newBlockOffset + column.Value);
                }
            }
            
        }
        
        public void Collapse()
        {
            // collapse the blocks
            foreach (var block in _blocksToCollapse)
            {
                CollapseBlock(block.Key, block.Value);
            }
            
            // collapse the new blocks
            foreach (var block in _newBlocksToCollapse)
            {
                CollapseBlock(block.Key, block.Value);
            }
            _blockAnimator.PlayBlockAnimation();
            CleanCollections();
        }
        #endregion

        #region Private Methods
        private void CollapseBlock(Vector2Int blockPosition, int rowsToCollapse)
        {
            TileBase fallingTile;
            int blockType;

            if (blockPosition.y < rows) // If the block is an existing block
            {
                fallingTile = _tilemapController.GetTile(blockPosition);
                blockType = _blockTileStorageSo.GetBlockType(fallingTile);
                fallingTile = _defaultBlockTiles[blockType];
                _tilemapController.ClearTile(blockPosition);
            }
            else
            {
                blockType = Random.Range(0, _totalBlockTypes);
                fallingTile = _defaultBlockTiles[blockType];
            }

            Vector3Int targetPos = new Vector3Int(blockPosition.x, blockPosition.y - rowsToCollapse, 0);
            _blockAnimator.AddBlockToAnimate((Vector3Int)blockPosition, targetPos, fallingTile, blockType, () =>
            {
                CollapseEnded?.Invoke(); 
            });
        }



        private void CleanCollections()
        {
            // clean the collections without garbage collection
            _newBlocksInCols.Clear();
            _blocksToCollapse.Clear();
            _newBlocksToCollapse.Clear();
        }
        #endregion
        
    }

}