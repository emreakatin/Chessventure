using UnityEngine;
using ThirteenPixels.Soda;
using UnityEngine.UI;
using DG.Tweening;
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
    [SerializeField] private GameEvent _onGameLevelLoadRequest;
    [SerializeField] private GameEvent _onLevelCompleteRequest;
    [SerializeField] private GameEvent _onExitRequest;
    [SerializeField] private MenuUIElementType _menuUIElementType;
    private float _moveDuration = 1f;
 private float m_defaultPosition;
    public GameEvent _onPlayerDied;


    private void Awake()
    {
        if(_button != null)
        {
            _button.onClick.AddListener(OnButtonClicked);
        }
        
         m_defaultPosition = GetComponent<RectTransform>().rect.height;   
        if(_menuUIElementType == MenuUIElementType.RetryButton)
        {
           
            GetInactive(  );
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
        GetActive();
    }

    private void OnButtonClicked()
    {
        if(_menuUIElementType == MenuUIElementType.LevelStartButton) 
        {
            _onLevelStartRequest.Raise();
            GetInactive();
        }
        else if(_menuUIElementType == MenuUIElementType.LevelCompleteButton)
        {
            _onLevelCompleteRequest.Raise();
        }
        else if(_menuUIElementType == MenuUIElementType.RetryButton)
        {
            //_onRetryRequest.Raise();
            GetInactive();
        }

    }



     private void GetActive()
        {
        
            Tween t = GetComponent<RectTransform>().DOMoveY(0, _moveDuration).OnComplete(() => Invoke("EnableButton", 1f));
            t.Play();
        }

        private void EnableButton()
        {
            _button.enabled = true;
        }

        private void GetInactive()
        {
             _button.enabled = false;
            Tween t = GetComponent<RectTransform>().DOMoveY(m_defaultPosition * 2f, _moveDuration);
            t.SetLink(gameObject);
            t.Play();
        }
}
