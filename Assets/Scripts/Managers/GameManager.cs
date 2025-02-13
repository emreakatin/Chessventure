﻿using Sirenix.OdinInspector;
using ThirteenPixels.Soda;
using UnityEngine;

namespace GameCore.Managers
{
    public class GameManager : MonoBehaviour
    {
        private const string current_level_index_key = "LevelIndex";

        public static GameManager Instance;

        [Title("Events")]
    
        [SerializeField] private GameEvent _onGameLevelLoadRequest;
        [SerializeField] private GameEvent _onGameLevelStartRequest;
        [SerializeField] private GameEvent _onGameLevelCompleteRequest;
        [SerializeField] private GameEvent _onRetryGameRequest;
        [SerializeField] private GameEvent _onGameStateChanged;

        [Header("References")]
        [SerializeField] private GameObject _mainMenuPrefab;
        [SerializeField] private GameObject[] _gameLevels;
        [SerializeField] private GameObject[] _metaLevels;

        private GameObject m_currentLevel;

        private bool gameStarted = false;
        public bool GameStarted 
        { 
            get { return gameStarted; } 
            set 
            { 
                gameStarted = value;
                if (_onGameStateChanged != null)
                    _onGameStateChanged.Raise();
            } 
        }

        public static int LevelIndex
        {
            get
            {
                return s_globalLevelCount.value - 1;
            }
        }
        
        private static GlobalInt s_globalLevelCount;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            Load();
            SpawnGameLevel();
            _onGameLevelLoadRequest.Raise();
        }

        private void Update()
        {
            // Test için next level
            if (Input.GetKeyDown(KeyCode.P))
            {
                LoadNextLevel();
            }

            // Oyun başlatma
           /*  if (Input.GetKeyDown(KeyCode.Space))
            {
                _onGameLevelStartRequest.Raise();
                Debug.Log("Game started!");
            } */
        }

        private void OnEnable()
        {
            _onGameLevelLoadRequest.onRaise.AddListener(SpawnGameLevel);
            _onRetryGameRequest.onRaise.AddListener(SpawnGameLevel);
        }

        private void OnDisable()
        {
            _onGameLevelLoadRequest.onRaise.RemoveListener(SpawnGameLevel);
            _onRetryGameRequest.onRaise.RemoveListener(SpawnGameLevel);
        }
        
        private void Load()
        {
            s_globalLevelCount = Resources.Load<GlobalInt>(ResourceDirectories.GLOBAL_META_LEVEL_COUNT);
            s_globalLevelCount.value = PlayerPrefs.GetInt(current_level_index_key, 1);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(current_level_index_key, s_globalLevelCount.value);
        }

        public void LoadNextLevel()
        {
            s_globalLevelCount.value++;
            Save();
            SpawnGameLevel();
        }

        private void ResetGame()
        {
            s_globalLevelCount.value = 1; // Başlangıç seviyesine döndür
            Save();
            SpawnGameLevel();
            Debug.Log("Game reset to initial level.");
        }

        private void SpawnGameLevel()
        {
            Save();
            DestroyCurrentLevel();

            // Level indeksini döngüsel hale getir
            int levelIndex = (s_globalLevelCount.value - 1) % _gameLevels.Length;
            
            GameObject prefab = _gameLevels[levelIndex];
            m_currentLevel = Instantiate(prefab);
        }

        private void SpawnMetaLevel()
        {
            DestroyCurrentLevel();
            
            int index = s_globalLevelCount.value;
            if (index > _metaLevels.Length)
                index = _metaLevels.Length;

            GameObject prefab = _metaLevels[index - 1];
            m_currentLevel = Instantiate(prefab);
        }

        private void DestroyCurrentLevel()
        {
            if (m_currentLevel != null)
                Destroy(m_currentLevel);
        }
    }
}