using System;
using BaseComponents;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features
{
    public class BlockAnimator
    {
        private readonly BlockPool _blockPool;
        private readonly TilemapController _tilemapController;
        private Sequence _sequence;
        public BlockAnimator(TilemapController tilemapController, BlockPool blockPool)
        {
            _tilemapController = tilemapController;
            _blockPool = blockPool;
            DOTween.Init(); 
            _sequence = DOTween.Sequence();
        }

        public void AddBlockToAnimate(Vector3Int tilePosition, Vector3Int targetPosition, TileBase newTile, int newBlockType, Action AnimationEnded)
        {
            // optimize this later
            if (_sequence.IsActive() == false || _sequence.IsPlaying() == false)
            {
                _sequence.Kill(); 
                _sequence = DOTween.Sequence();
                _sequence.SetRecyclable(true).SetAutoKill(false);
            }
            
            GameObject animationBlock = _blockPool.GetBlock(newBlockType);
            animationBlock.transform.position = _tilemapController.GetCellCenterWorld(tilePosition);
            Vector3 targetWorldPos = _tilemapController.GetCellCenterWorld(targetPosition);
            
            float animationDuration = 0.5f;
            
            _sequence.Join(animationBlock.transform.DOMove(targetWorldPos, animationDuration).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                    OnAnimationComplete(animationBlock, newTile, newBlockType, targetPosition, AnimationEnded))
            );
        
        _sequence.OnComplete(() => AnimationEnded?.Invoke());
        }

        public void PlayBlockAnimation()
        {
            _sequence.Play();
        }

        
        private void OnAnimationComplete(GameObject animationBlock, TileBase newTile, int newBlockType, Vector3Int targetPosition, Action AnimationEnded)
        {
            _tilemapController.SetTile(targetPosition, newTile);
            _blockPool.ReturnBlock(animationBlock, newBlockType);
            AnimationEnded?.Invoke();
        }
    }
}