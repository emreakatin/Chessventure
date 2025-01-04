using UnityEngine;
using System;
using System.Collections;
using ThirteenPixels.Soda;


namespace GameCore.Managers
{
    public class SkyboxManager : MonoBehaviour
    {
        [Serializable]
        public class LevelSkyboxData
        {
            public int levelIndex;
            public Material skyboxMaterial;
        }

        #region Serialized Fields
        [SerializeField] private GameEvent onGameLevelLoadRequest;

        [SerializeField] private LevelSkyboxData[] levelSkyboxes;
        #endregion

        #region Private Variables
        private Material currentSkybox;
        #endregion

        #region MonoBehaviour Methods

         private void Awake()
        {
            DontDestroyOnLoad(this);
        }
       private void OnEnable()
        {
            onGameLevelLoadRequest.onRaise.AddListener(HandleLevelChange);
        }

        private void OnDisable()
        {
            onGameLevelLoadRequest.onRaise.RemoveListener(HandleLevelChange);
        }
        private void HandleLevelChange()
        {
            int levelIndex = 1;
            SetSkyboxForLevel(levelIndex);
        }
        #endregion

        #region Public Methods
        public void SetSkyboxForLevel(int levelIndex)
        {
            var skyboxData = Array.Find(levelSkyboxes, data => data.levelIndex == levelIndex);
            
            if (skyboxData != null)
            {
                SetSkybox(skyboxData.skyboxMaterial);
            }
            else
            {
                Debug.LogWarning($"Level {levelIndex} için skybox bulunamadı!");
            }
        }

        public void FadeToSkybox(Material newSkybox, float duration)
        {
            if (newSkybox == null)
            {
                Debug.LogError("Fade işlemi için geçerli bir skybox material'ı gerekli!");
                return;
            }

            StartCoroutine(FadeSkyboxRoutine(newSkybox, duration));
        }
        #endregion

        #region Private Methods
        private void SetSkybox(Material newSkybox)
        {
            if (newSkybox != null)
            {
                currentSkybox = newSkybox;
                RenderSettings.skybox = currentSkybox;
                //DynamicGI.UpdateEnvironment();
            }
            else
            {
                Debug.LogError("Geçerli bir skybox material'ı gerekli!");
            }
        }

        private IEnumerator FadeSkyboxRoutine(Material newSkybox, float duration)
        {
            Material oldSkybox = RenderSettings.skybox;
            Material transitionMaterial = new Material(oldSkybox);
            RenderSettings.skybox = transitionMaterial;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                transitionMaterial.Lerp(oldSkybox, newSkybox, t);
                yield return null;
            }

            RenderSettings.skybox = newSkybox;
            currentSkybox = newSkybox;
            DynamicGI.UpdateEnvironment();
            Destroy(transitionMaterial);
        }
        #endregion
    }
} 