#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace _game.Scripts.LevelSystem
{
    public class LevelEditor : EditorUtility
    {
        [MenuItem("GameObject/Create Level", false, -10)]
        private static void CreateLevel()
        {
            var level = new GameObject("Level").AddComponent<LevelCreator>();
            Selection.activeGameObject = level.gameObject;
        }
    }
}

#endif