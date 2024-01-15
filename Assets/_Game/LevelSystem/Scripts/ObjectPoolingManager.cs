using System.Collections.Generic;
using _game.LevelSystem.Serializables;
using _game.Scripts.SerializedObjects;
using _game.Scripts.SOs;
using Assets._Game.BaseClasses;
using UnityEngine;

namespace _game.Scripts.LevelSystem
{
    public class ObjectPoolingManager : MonoSingleton<ObjectPoolingManager>
    {
        public PoolingKeyStore PoolingKeyStore;
        
        private List<ObjectPool> m_objectPool;
        private ObjectPool _pool;
        private GameObject _objectGameObject;
        private GameObject _levelObject;

        private void Awake()
        {
            m_objectPool = new List<ObjectPool>();
        }

        public void FreeAllObject()
        {
            foreach (var objectPool in m_objectPool)
            {
                objectPool.FreeAll();
            }
        }

        public void  SpawnObjects(ObjectData[] objectData, Transform spawnPoint)
        {
            FreeAllObject();
            foreach (var obj in objectData)
            {
                _pool = m_objectPool.Find(data => data.key == obj.key);
                if (_pool == null)
                {
                    var newPool = new ObjectPool();
                    if (PoolingKeyStore._serializer.TryGetPart(obj.key, ref _objectGameObject))
                    {
                        newPool.InitPool(_objectGameObject, spawnPoint);
                        newPool.key = obj.key;
                    }
                    else
                    {
                        continue;
                    }

                    m_objectPool.Add(newPool);
                    _levelObject = newPool.GetObject();
                    obj.transformSerializable.Apply(_levelObject.transform);
                    if (_levelObject.TryGetComponent<IObjectData>(out var newObjectData))
                    {
                        newObjectData.LoadData(obj.Data);
                    }
                    continue;
                }
                
                _levelObject = _pool.GetObject();
                obj.transformSerializable.Apply(_levelObject.transform);
                if (_levelObject.TryGetComponent<IObjectData>(out var poolObjectData))
                {
                    poolObjectData.LoadData(obj.Data);
                }
                
            }
        }
#if UNITY_EDITOR
        public void Editor_SpawnObjects(ObjectData[] objectData, Transform spawnPoint)
        {
            m_objectPool = new List<ObjectPool>();
            foreach (var obj in objectData)
            {
                _pool = m_objectPool.Find(data => data.key == obj.key);
                if (_pool == null)
                {
                    var newPool = new ObjectPool();
                    if (PoolingKeyStore._serializer.TryGetPart(obj.key, ref _objectGameObject))
                    {
                        newPool.InitPool(_objectGameObject, spawnPoint);
                        newPool.key = obj.key;
                    }
                    else
                    {
                        continue;
                    }

                    m_objectPool.Add(newPool);
                    _levelObject = newPool.Editor_GetObject(spawnPoint);
                    obj.transformSerializable.Apply(_levelObject.transform);
                    if (_levelObject.TryGetComponent<IObjectData>(out var newObjectData))
                    {
                        newObjectData.LoadData(obj.Data);
                    }
                    continue;
                }
                
                _levelObject = _pool.Editor_GetObject(spawnPoint);
                obj.transformSerializable.Apply(_levelObject.transform);
                if (_levelObject.TryGetComponent<IObjectData>(out var poolObjectData))
                {
                    poolObjectData.LoadData(obj.Data);
                }
            }
        }
#endif
        
    }
}