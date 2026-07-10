using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Window : MonoBehaviour, IWindowController
{
    private const string MenuPauseKey = "WindowPause";

    [SerializeField] private CanvasGroup _windowGroup;
    [SerializeField] private Button _exitButton;

    public event Action<string> OpenedWindow;
    public event Action<string> ClosedWindow;

    protected CanvasGroup WindowGroup => _windowGroup;
    protected Button ExitButton => _exitButton;

    protected virtual void OnEnable()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    protected virtual void OnDisable()
    {
        _exitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    public virtual void Close()
    {
        WindowGroup.alpha = 0f;
        WindowGroup.blocksRaycasts = false;
        ExitButton.interactable = false;
        RayCastController.SetRayCastEnabled(true);
        ClosedWindow?.Invoke(MenuPauseKey);
    }

    public virtual void Open()
    {
        WindowGroup.alpha = 1f;
        WindowGroup.blocksRaycasts = true;
        ExitButton.interactable = true;
        RayCastController.SetRayCastEnabled(false);
        OpenedWindow?.Invoke(MenuPauseKey);
    }

    protected abstract void OnExitButtonClick();
}