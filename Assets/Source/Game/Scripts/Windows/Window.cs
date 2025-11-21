using UnityEngine;
using UnityEngine.UI;

internal abstract class Window : MonoBehaviour
{
    [SerializeField] private CanvasGroup _windowGroup;
    [SerializeField] private Button _actionButton;

    protected CanvasGroup WindowGroup => _windowGroup;
    protected Button ActionButton => _actionButton;

    protected virtual void OnEnable()
    {
        _actionButton.onClick.AddListener(OnButtonClick);
    }

    protected virtual void OnDisable()
    {
        _actionButton.onClick.RemoveListener(OnButtonClick);
    }

    internal virtual void Close()
    {
        WindowGroup.alpha = 0f;
        WindowGroup.blocksRaycasts = false;
        ActionButton.interactable = false;
    }

    internal virtual void Open()
    {
        WindowGroup.alpha = 1f;
        WindowGroup.blocksRaycasts = true;
        ActionButton.interactable = true;
    }

    protected abstract void OnButtonClick();
}