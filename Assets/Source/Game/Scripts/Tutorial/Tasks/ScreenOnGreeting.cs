using System;
using UnityEngine;
using UnityEngine.UI;

internal class ScreenOnGreeting : Window
{
    [SerializeField] private Button _skipButton;

    internal event Action ExitButtonClicked;
    internal event Action SkipButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _skipButton.onClick.AddListener(OnSkipButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _skipButton.onClick.RemoveListener(OnSkipButtonClick);
    }

    protected override void OnButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }

    private void OnSkipButtonClick()
    {
        SkipButtonClicked?.Invoke();
    }
}