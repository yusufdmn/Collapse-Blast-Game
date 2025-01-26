using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Settings/Level Settings")]
    public class LevelSettings: ScriptableObject
    {
        [Header("Grid Settings")]
        public byte Rows;
        public byte Columns;
        public byte TotalTileTypes;
    }
}
