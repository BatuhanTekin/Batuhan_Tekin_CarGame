using _game.BaseClasses;
using Assets._Game.BaseClasses;
using Assets._Game.Managers;
using System;
using _Game.CarSystem.Scripts;
using Assets._Game.LevelSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets._Game.CarSystem
{
    public class CarManager : MonoSingleton<CarManager>
    {
        [SerializeField] private AutomaticCar m_carController;
        [SerializeField] private PlayerCar m_playerCar;
        [SerializeField] private float m_carHeight = 0.05f;

        private AutomaticCar[] _cars = new AutomaticCar[10];
        private PoolingSystem<AutomaticCar> _carPool;

        private int _carCount;
        private (Quaternion , Vector3)[] _savedData;
        private (int, CarLevelData) _levelData;
        private bool _startLevel;

        private void Start()
        {
            _carPool = new PoolingSystem<AutomaticCar>(transform, m_carController);
        }

        private void OnEnable()
        {
            GameManager.Instance.OnStart += StartMovement;
            GameManager.Instance.OnLevelInitialized += SpawnNewLevel;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnStart -= StartMovement;
            GameManager.Instance.OnLevelInitialized -= SpawnNewLevel;
        }

        public void OnCarFinish(bool isPlayerCar, (Quaternion , Vector3)[] savedData)
        {
            _startLevel = false;
            
            if (isPlayerCar)
            {
                if (_carCount >= _levelData.Item1)
                {
                    GameManager.Instance.OnLevelFinish();
                    return;
                }
                
                _savedData = savedData;
                for (var i = 0; i < _carCount; i++)
                {
                    _cars[i].OnRetry();
                }

                AddCar();
                _levelData = GameManager.Instance.GetLevelData(_carCount);
                SetPlayer();
            }
        }

        private void AddCar()
        {
            var car = _carPool.GetFromPool();
            car.gameObject.SetActive(true);
            car.SetCar(_savedData, m_playerCar.GetStartPosition());
            
            _cars[_carCount] = car;
            _carCount++;
        }

        public void OnFail()
        {
            _startLevel = false;

            m_playerCar.OnRetry();
            for (var i = 0; i < _carCount; i++)
            {
                _cars[i].OnRetry();
            }
        }

        private void SpawnNewLevel()
        {
            for (var i = 0; i < _carCount; i++)
            {
                _cars[i].ResetCar();
                _carPool.ReturnPool(_cars[i]);
            }


            _carCount = 0;
            _levelData = GameManager.Instance.GetLevelData(_carCount);

            SetPlayer();
            Array.Clear(_cars, 0, _cars.Length);
        }

        private void SetPlayer()
        {
            m_playerCar.ResetCar();
            var target = _levelData.Item2;
            var pos = target.StartPosition;
            pos.y = m_carHeight;

            m_playerCar.transform.SetPositionAndRotation(pos, target.StartRotation);
            m_playerCar.SetCar(_carCount);
        }

        private void StartMovement()
        {
            if (_startLevel)
            {
                return;
            }
            
            _startLevel = true;
            m_playerCar.OnGameStart();

            for (var i = 0; i < _carCount; i++)
            {
                _cars[i].OnGameStart();
            }
        }

#if UNITY_EDITOR
        [Button]
        public void Test_SpawnCar()
        {
            _savedData = m_playerCar.Test_GetData();
            var car = _carPool.GetFromPool();
            car.SetCar(_savedData, m_playerCar.GetStartPosition());
            car.OnGameStart();
        }
#endif
    }
}