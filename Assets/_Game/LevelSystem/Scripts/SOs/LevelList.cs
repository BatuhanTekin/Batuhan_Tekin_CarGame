using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _game.Scripts.SOs
{
    [CreateAssetMenu(fileName = "LevelList", menuName = "ScriptableObjects/Level/LevelList", order = 1)]
    public class LevelList : ScriptableObject
    {
        [SerializeField] private List<Level> m_levels;

        public Level SpawnNextLevel(int nextLevelIndex)
        {
            var level = nextLevelIndex % m_levels.Count;
            return m_levels[level];
        }
        

#if UNITY_EDITOR
        public void AddList(Level level)
        {
            m_levels.Add(level);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}