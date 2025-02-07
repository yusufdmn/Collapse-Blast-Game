using System.Collections.Generic;
using BaseComponents;
using Data;
using Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features
{
    public class IconUpdater
    {
        public delegate void IconUpdateEndedDelegate();
        public event IconUpdateEndedDelegate IconUpdateEnded;
        
        #region Fields
        
        private readonly BlockTilesSO _blockTilesSoA;
        private readonly BlockTilesSO _blockTilesSoB;
        private readonly BlockTilesSO _blockTilesSoC;
    
        private readonly TilemapController _tilemapController;
        private int[] _groupBlockTypes = new int[3]; // pre-allocated array with initial size of arbitrary 3
        private readonly int A, B, C;

        #endregion
        
        public IconUpdater (BlockTileStorageSO blockTileStorageSo, LevelSettings levelSettings, TilemapController tilemapController)
        {
            _blockTilesSoA = blockTileStorageSo.BlockTilesSoA;
            _blockTilesSoB = blockTileStorageSo.BlockTilesSoB;
            _blockTilesSoC = blockTileStorageSo.BlockTilesSoC;
            A = levelSettings.A;
            B = levelSettings.B;
            C = levelSettings.C;
            _tilemapController = tilemapController;
        }

        #region Public API

        public void UpdateIcons(List<List<Vector2Int>> allGroups, int[,] grid)
        {
            UpdateBlockTypesOfGroups(allGroups, grid);
        
            for(int i = 0 ; i < allGroups.Count; i++)
            {
                if(allGroups[i].Count > C)
                    UpdateGroupIcons(allGroups[i], _blockTilesSoC.Tiles, _groupBlockTypes[i]);
                else if(allGroups[i].Count > B)
                    UpdateGroupIcons(allGroups[i], _blockTilesSoB.Tiles, _groupBlockTypes[i]);
                else if(allGroups[i].Count > A)
                    UpdateGroupIcons(allGroups[i], _blockTilesSoA.Tiles, _groupBlockTypes[i]);
            }
            IconUpdateEnded?.Invoke();
        }
        #endregion
    
    
        #region Private Methods
        
        private void UpdateBlockTypesOfGroups(List<List<Vector2Int>> groups, int[,] grid)
        {
            // Only resize if necessary
            if (_groupBlockTypes == null || _groupBlockTypes.Length < groups.Count)
            {
                _groupBlockTypes = new int[groups.Count];
            }

            // Update the block types of the groups
            for (int i = 0; i < groups.Count; i++)
            {
                _groupBlockTypes[i] = grid[groups[i][0].y, groups[i][0].x];
            }
        }


        private void UpdateGroupIcons(List<Vector2Int> group, TileBase[] tiles, int blockType)
        {
            foreach (var tile in group)
            {
                var tilePosition = new Vector3Int(tile.x, tile.y, 0);
                _tilemapController.SetTile(tilePosition, tiles[blockType]);
            }
        }
        
        #endregion

    }
}