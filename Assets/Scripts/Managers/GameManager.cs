using Sirenix.OdinInspector;
using ThirteenPixels.Soda;
using UnityEngine;

namespace GameCore.Managers
{
    public class GameManager : MonoBehaviour
    {
        private const string current_level_index_key = "LevelIndex";

        [Title("Events")]
        [SerializeField]
        private GameEvent _onMetaLevelLoadRequest;

        [SerializeField]
        private GameEvent _onGameLevelLoadRequest;

        [Header("References")]
        [SerializeField]
        private GameObject _mainMenuPrefab;

        //private CoinManager _coinManager;

        [SerializeField]
        private GameObject[] _gameLevels;

        [SerializeField]
        private GameObject[] _metaLevels;

        private GameObject m_currentLevel;

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
            DontDestroyOnLoad(this);
            
            Load();
            SpawnGameLevel();

            _onGameLevelLoadRequest.Raise();

            //_coinManager.Initialize();
            //SpawnMainMenu();
            
        }
        
        //write a functions to collect coins
        //write a functions to collect powerups
            
    
        private void OnEnable()
        {
            _onMetaLevelLoadRequest.onRaise.AddListener(SpawnMetaLevel);
            _onGameLevelLoadRequest.onRaise.AddListener(SpawnGameLevel);
        }

        private void OnDisable()
        {
            _onMetaLevelLoadRequest.onRaise.RemoveListener(SpawnMetaLevel);
            _onGameLevelLoadRequest.onRaise.RemoveListener(SpawnGameLevel);
        }
        
        private void Load()
        {
            s_globalLevelCount = Resources.Load<GlobalInt>(ResourcesDirectiories.GLOBAL_META_LEVEL_COUNT);
            s_globalLevelCount.value = PlayerPrefs.GetInt(current_level_index_key, 1);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(current_level_index_key, s_globalLevelCount.value);
        }

        private void SpawnMainMenu()
        {
            DestroyCurrentLevel();

            Instantiate(_mainMenuPrefab);
        }

        private void SpawnGameLevel()
        {
            Save();
            DestroyCurrentLevel();

            if (s_globalLevelCount.value - 1 >= _gameLevels.Length)
            {
                s_globalLevelCount.value = _gameLevels.Length - 1;
            }
            
            GameObject prefab = _gameLevels[s_globalLevelCount.value-1];
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