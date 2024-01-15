using System;
using _Game.CarSystem.Scripts;
using _game.LevelSystem.Serializables;
using Assets._Game.LevelSystem;
using Assets._Game.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Interacts.Scripts
{
    public class CarTargetController : LevelObjectBase
    {
        [SerializeField] private GameObject m_textObjects;
        [SerializeField] private Collider m_collider;
        [SerializeField] private Transform m_startTransform;
        [SerializeField] private int m_order;
        
        private CarLevelData m_baseData = new();


        private void OnEnable()
        {
            GameManager.Instance.OnTargetUpdated += CheckOpenOrder;
        }

        private void CheckOpenOrder(int order)
        {
            var result = (order == m_order);
            m_textObjects.SetActive(result);
            m_collider.enabled = result;
            
        }

        private void Save()
        {
            m_baseData.StartRotation = m_startTransform.rotation;
            m_baseData.StartPosition = m_startTransform.position;
            m_baseData.Order = m_order;
        }

        public override void SetData()
        {
            Save();
            _objectData.Data = JsonUtility.ToJson(m_baseData);
        }

        public override void LoadData(string data)
        {
            m_baseData = JsonUtility.FromJson<CarLevelData>(data);
            m_startTransform.position = m_baseData.StartPosition;
            m_startTransform.rotation = m_baseData.StartRotation;
            m_order = m_baseData.Order;
        }

        public int GetOrder()
        {
            return m_baseData.Order;
        }

        public CarLevelData GetData()
        {
            return m_baseData;
        }
        


#if UNITY_EDITOR

        [SerializeField] private Mesh m_carMesh;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawMesh(m_carMesh, m_startTransform.position, m_startTransform.rotation);
        }
#endif
    }
}