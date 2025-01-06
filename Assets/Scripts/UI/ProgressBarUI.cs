using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ThirteenPixels.Soda;
public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Slider progressBar; // Reference to the Slider component
    [SerializeField] private Player player; // Reference to the Player to get enemy count
     [SerializeField] private GameEvent _onEnemyDied;

    private void Start()
    {
        progressBar.value = 0; // Initialize the progress bar
    }
    
    private void OnEnable(){
        _onEnemyDied.onRaise.AddListener(IncrementProgress);
        player._onLevelUp.onRaise.AddListener(PlayerLevelUp);
    }
    
     private void OnDisable(){
        _onEnemyDied.onRaise.RemoveListener(IncrementProgress);
         player._onLevelUp.onRaise.RemoveListener(PlayerLevelUp);
    }


    private void Update()
    {
        // Update progress bar based on defeated enemies
       
    }

    private void PlayerLevelUp(){
      
        // Reset for next level if the goal is reached
        //enemiesDefeated = 0;
        IncrementProgress();

    }

    public void IncrementProgress()
    {
        if (player.killCount < player.CharacterData.requiredKillsToLevelUp)
        {
            //enemiesDefeated++;
            float progress = (float)player.killCount / player.CharacterData.requiredKillsToLevelUp;
            StartCoroutine(UpdateProgressBar(progress));
        }
    }

    private IEnumerator UpdateProgressBar(float targetProgress)
    {
        float startValue = progressBar.value;
        float time = 0.5f; // Smooth transition duration
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            progressBar.value = Mathf.Lerp(startValue, targetProgress, elapsedTime / time);
            yield return null; // Wait for the next frame
        }

        progressBar.value = targetProgress; // Ensure the final value is set
    }
}