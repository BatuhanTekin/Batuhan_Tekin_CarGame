using System.Collections.Generic;
using _game.LevelSystem.Serializables;
using _game.Scripts.LevelSystem;
using Assets._Game.LevelSystem;
using UnityEngine;
using _Game.Interacts.Scripts;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif

namespace _game.Scripts.SOs
{
    [CreateAssetMenu(fileName = "LevelSo", menuName = "ScriptableObjects/Level/LevelSo", order = 1)]
    public class Level : ScriptableObject
    {
        public ObjectData[] _objectData = new ObjectData[]{};
        public CarLevelData[] _carBaseData = new CarLevelData[]{};

        public ObjectData[]  GetObjectDatas()
        {
            return _objectData;
        }
        
        public CarLevelData[] GetCarBaseDatas()
        {
            return _carBaseData;
        }
        
#if UNITY_EDITOR
        [Button]
        public void Editor_StoreObjectsData(Transform transform, PoolingKeyStore poolingKeyStore)
        {
            List<ObjectData> objectDatas = new List<ObjectData>();
            List<CarLevelData> carBaseDatas = new List<CarLevelData>();
            foreach (Transform levelObject in transform.transform)
            {
                if (levelObject.TryGetComponent<CarTargetController>(out var target))
                {
                    carBaseDatas.Add(target.GetData());
                }
                
                if (levelObject.TryGetComponent<IObjectData>(out var data))
                {
                    if (poolingKeyStore._serializer.TryGetKey(levelObject.gameObject, out var key))
                    {
                        data.GetObjectData().key = key;
                    }
                    else
                    {
                        Debug.LogError("[Level] Object Not Found");
                        continue;
                    }

                    data.GetObjectData().transformSerializable.SetLocal(levelObject);
                    data.SetData();
                    objectDatas.Add(data.GetObjectData());
                }

                _objectData = objectDatas.ToArray();
                _carBaseData = carBaseDatas.ToArray();
                EditorUtility.SetDirty(this);
                AssetDatabase.Refresh();
            }
        }

        [Button]
        public void Editor_SpawnLevelForEdit()
        {
            var poolingManager = FindObjectOfType<ObjectPoolingManager>();
            if (poolingManager == null)
            {
                EditorUtility.DisplayDialog("Warning","Please Add ObjectPoolingManager To Scene","Okay");
                return;
            }
            var levelObject = new GameObject(name);
            levelObject.AddComponent<LevelCreator>();
            
            poolingManager.Editor_SpawnObjects(_objectData, levelObject.transform);
        }
#endif

        public void SetPathDatas(ObjectData[] levelData, CarLevelData[] carBaseData)
        {
            _objectData = levelData;
            _carBaseData = carBaseData;
        }
        
    }
}



