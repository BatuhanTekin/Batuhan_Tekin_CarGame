using _Game.CarSystem.Scripts;
using _game.Scripts.SOs;
using Assets._Game.LevelSystem;
using Assets._Game.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.LevelSystem
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Transform m_pathParent;
        [SerializeField] private LevelList m_levelList;

        private Level _level;
        private CarLevelData[] _carData;


        [Button]
        public void SpawnLevel()
        {
            _level = m_levelList.SpawnNextLevel(GameManager.Instance.GetLevelIndex());
            ObjectPoolingManager.Instance.SpawnObjects(_level.GetObjectDatas(), m_pathParent);
            m_pathParent.name = _level.name;
            _carData = _level.GetCarBaseDatas();
        }

        public CarLevelData GetPoints(int carOrder)
        {
            return _carData[carOrder];
        }

        public int GetLevelMaxCar()
        {
            return _level.GetCarBaseDatas().Length;
        }
    }
}