using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public class TargetGroupFinder
    {
        // find the List that contains the target item
        public List<Vector2Int> GetTargetGroup(List<List<Vector2Int>> allGroups, Vector2Int targetItem)
        {
            foreach (var group in allGroups)
            {
                foreach (var tile in group)
                {
                    if (tile.Equals(targetItem)) // check for boxing allocation later
                    {
                        return group;
                    }
                }
            }
            return null;
        }

    }
}