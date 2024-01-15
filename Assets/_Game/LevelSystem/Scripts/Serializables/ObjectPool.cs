using System;
using System.Collections.Generic;
using _game.Scripts.LevelSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.SerializedObjects
{
    [Serializable]
    public class ObjectPool
    {
        public int key;
        
        [SerializeField] private Transform m_defaultParent;
        [SerializeField] private GameObject _defaultObject;
        [SerializeField] private Queue<GameObject> _freeObjects = new Queue<GameObject>();
        [SerializeField] private List<GameObject> _usedObjects = new List<GameObject>();

        public void InitPool(GameObject defaultObject, Transform defaultParent)
        {
            _defaultObject = defaultObject;
            m_defaultParent = defaultParent;
        }

        [Button]
        public GameObject GetObject()
        {
            if (_freeObjects.Count <= 0) GrowPool();
            var obj = _freeObjects.Dequeue();
            _usedObjects.Add(obj);
            obj.SetActive(true);
            foreach (var resetable in obj.GetComponentsInChildren<IResetable>())
            {
                resetable.ResetObject();
            }

            return obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(m_defaultParent);
            _freeObjects.Enqueue(obj);
            _usedObjects.Remove(obj);
        }

        private void GrowPool()
        {
            var obj = UnityEngine.Object.Instantiate(_defaultObject, m_defaultParent);
            obj.SetActive(false);
            _freeObjects.Enqueue(obj);
        }

        public void FreeAll()
        {
            while (_usedObjects.Count > 0)
            {
                ReturnToPool(_usedObjects[0]);
            }
        }

        public void DestroyAll()
        {
            foreach (var obj in _usedObjects)
            {
                UnityEngine.Object.Destroy(obj);
            }

            _usedObjects.Clear();

            foreach (var obj in _freeObjects)
            {
                UnityEngine.Object.Destroy(obj);
            }

            _freeObjects.Clear();
        }

#if UNITY_EDITOR
        public GameObject Editor_GetObject(Transform spawnPoint)
        {
            if (_freeObjects.Count <= 0) Editor_GrowPool(spawnPoint);
            var obj = _freeObjects.Dequeue();
            _usedObjects.Add(obj);
            obj.SetActive(true);
            foreach (var resetable in obj.GetComponentsInChildren<IResetable>())
            {
                resetable.ResetObject();
            }

            return obj;
        }
        
        private void Editor_GrowPool(Transform spawnPoint)
        {
            var obj = (GameObject) UnityEditor.PrefabUtility.InstantiatePrefab(_defaultObject);
            obj.transform.parent = spawnPoint;
            obj.SetActive(false);
            _freeObjects.Enqueue(obj);
        }

#endif
        
    }
}