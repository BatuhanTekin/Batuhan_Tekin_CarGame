#if UNITY_EDITOR
using _game.Scripts.SOs;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
namespace _game.Scripts.LevelSystem
{
    public class LevelCreator : MonoBehaviour
    {
        private static readonly string CONFIG_PATH = "Assets/_Game/LevelSystem/Sos/Levels";

        public LevelList LevelList;
        public PoolingKeyStore PoolingKeyStore;

        [Button]
        public void SaveLevel()
        {
            var path = CONFIG_PATH + $"/{transform.name}.asset";
            var levelConfig = ScriptableObject.CreateInstance<Level>();
            levelConfig.Editor_StoreObjectsData(transform, PoolingKeyStore);
            
            if (!PrivateIsAssetExist(path, out var asset))
            {
                AssetDatabase.CreateAsset(levelConfig, path);
                AddLevelToLevelList(levelConfig);
                EditorUtility.SetDirty(levelConfig);
            }
            else
            {
                asset.SetPathDatas(levelConfig.GetObjectDatas(), levelConfig.GetCarBaseDatas());
                EditorUtility.SetDirty(asset);
            }
            Selection.activeObject = levelConfig;
            AssetDatabase.Refresh();
        }

        private void AddLevelToLevelList(Level levelConfig)
        {
            LevelList.AddList(levelConfig);
        }

        private static bool PrivateIsAssetExist(string path, out Level config)
        {
            config = AssetDatabase.LoadAssetAtPath<Level>(path);
            return config != null;
        }

        
    }
}
#endif


