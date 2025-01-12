using UnityEngine;
using GameCore.Managers;

public class LevelEnd : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Level End");
            GameManager.Instance.LoadNextLevel();
        }
    }
}
