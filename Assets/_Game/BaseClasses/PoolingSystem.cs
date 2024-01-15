using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _game.BaseClasses
{
    public class PoolingSystem<T> where T : Object
    {
        private List<T> _pool;
        private T _item;
        private T _spawnItem;
        private Transform _mainTransform;

        public PoolingSystem(Transform transform, T item)
        {
            _spawnItem = item;
            _pool = new List<T>();
            _mainTransform = transform;
        }
        public void ReturnPool(T item)
        {
            _pool.Add(item);
        }

        public T GetFromPool()
        {
            _item = default;

            if (_pool.Count > 0)
            {
                _item = _pool.Last();
                _pool.RemoveAt(_pool.Count - 1);
                return _item;
            }

            _item = Object.Instantiate(_spawnItem, _mainTransform);
            return _item;
        }
    }
}