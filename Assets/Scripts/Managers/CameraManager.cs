using ThirteenPixels.Soda;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera mainCam;
    [SerializeField]  private CinemachineCamera playerCam;
    
    [SerializeField] private GameEvent _onGameLevelLoadRequest;
    [SerializeField] private GameEvent _onGameLevelStartRequest;
    
    private void OnEnable()
    {
        _onGameLevelLoadRequest.onRaise.AddListener(GameLoad);
        _onGameLevelStartRequest.onRaise.AddListener(GameStart);
    }

    private void OnDisable()
    {
        _onGameLevelLoadRequest.onRaise.RemoveListener(GameLoad);
        _onGameLevelStartRequest.onRaise.RemoveListener(GameStart);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameLoad()
    {
        mainCam.enabled = true;
        playerCam.enabled = false;
    }

    public void GameStart()
    {
        mainCam.enabled = false;
        playerCam.enabled = true;
    }
}
