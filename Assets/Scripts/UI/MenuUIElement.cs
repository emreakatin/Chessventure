using UnityEngine;
using ThirteenPixels.Soda;
using UnityEngine.UI;

public enum MenuUIElementType
{
    LevelStartButton,
    LevelCompleteButton,
    RetryButton,
}
public class MenuUIElement : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameEvent _onLevelStartRequest;
    [SerializeField] private GameEvent _onLevelCompleteRequest;
    [SerializeField] private GameEvent _onExitRequest;
    [SerializeField] private MenuUIElementType _menuUIElementType;

    public GameEvent _onPlayerDied;


    private void Awake()
    {
        if(_button != null)
        {
            _button.onClick.AddListener(OnButtonClicked);
        }
    }

    private void OnEnable()
    {
        if(_menuUIElementType == MenuUIElementType.RetryButton)
        {
            _onPlayerDied.onRaise.AddListener(OnPlayerDied);
        }
    }

    private void OnDisable()
    {
        if(_menuUIElementType == MenuUIElementType.RetryButton)
        {
            _onPlayerDied.onRaise.RemoveListener(OnPlayerDied);
        }
    }

    private void OnPlayerDied()
    {
        UIActivation(true);
    }

    private void OnButtonClicked()
    {
        if(_menuUIElementType == MenuUIElementType.LevelStartButton) 
        {
            _onLevelStartRequest.Raise();
            UIActivation(false);
        }
        else if(_menuUIElementType == MenuUIElementType.LevelCompleteButton)
        {
            _onLevelCompleteRequest.Raise();
        }
        else if(_menuUIElementType == MenuUIElementType.RetryButton)
        {
            //_onRetryRequest.Raise();
            UIActivation(false);
        }

    }

    public void UIActivation(bool isActive)
    {
        Debug.Log("UIActivation: " + isActive);
        //transform.domove
        gameObject.SetActive(isActive);
    }
}
