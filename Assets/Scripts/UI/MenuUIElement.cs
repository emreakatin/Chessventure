using UnityEngine;
using ThirteenPixels.Soda;
using UnityEngine.UI;

public enum MenuUIElementType
{
    LevelStartButton,
    LevelCompleteButton,
    ExitButton,
    ProgressBar
}
public class MenuUIElement : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameEvent _onLevelStartRequest;
    [SerializeField] private GameEvent _onLevelCompleteRequest;
    [SerializeField] private GameEvent _onExitRequest;
    [SerializeField] private MenuUIElementType _menuUIElementType;
    private void Awake()
    {
        if(_button != null)
        {
            _button.onClick.AddListener(OnButtonClicked);
        }
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
        else if(_menuUIElementType == MenuUIElementType.ExitButton)
        {
            _onExitRequest.Raise();
        }
    }

    public void UIActivation(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
