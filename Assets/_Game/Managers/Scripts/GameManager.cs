using Assets._Game.BaseClasses;
using Assets._Game.InputSystem;
using Assets._Game.LevelSystem;
using System;
using _game.Scripts.LevelSystem;
using TMPro;
using UnityEngine;

namespace Assets._Game.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private const string LevelPref = "LevelPref";
        
        public event Action OnStart;
        public event Action OnLevelInitialized;
        public event Action<int> OnTargetUpdated;

        [SerializeField] private InputActionSo m_inputActionSo;
        [SerializeField] private LevelController m_levelManager;
        [SerializeField] private TextMeshProUGUI m_levelText;
        
        private int _level;
        
        private void OnEnable()
        {
            m_inputActionSo.AddTouchListener(OnTouch);
        }

        private void Start()
        {
            SetLevel(); 
            SpawnLevel();
        }

        public void OnLevelFinish()
        {
            NextLevel();
            SpawnLevel();
        }


        public (int, CarLevelData) GetLevelData(int order)
        {
            OnTargetUpdated?.Invoke(order);
            return (m_levelManager.GetLevelMaxCar() - 1, m_levelManager.GetPoints(order));
        }

        public int GetLevelIndex()
        {
            return PlayerPrefs.GetInt(LevelPref, _level);
        }

        private void OnTouch(bool enable)
        {
            OnStart?.Invoke();
        }

        private void SpawnLevel()
        {
            m_levelManager.SpawnLevel();
            OnLevelInitialized?.Invoke();
            m_levelText.text = $"LEVEL {_level + 1}";
        }

        private void SetLevel()
        {
            _level = GetLevelIndex();
        }

        private void SaveLevel()
        {
            PlayerPrefs.SetInt(LevelPref, _level);
        }

        private void NextLevel()
        {
            _level++;
            SaveLevel();
        }
    }
}