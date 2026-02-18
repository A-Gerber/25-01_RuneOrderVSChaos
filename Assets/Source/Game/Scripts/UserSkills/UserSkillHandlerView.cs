using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class UserSkillHandlerView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _windowGroup;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _resetButton;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private UserSkillHandler _userSkillHandler;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(Close);
        _resetButton.onClick.AddListener(OnReset);
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(Close);
        _resetButton.onClick.RemoveListener(OnReset);
    }

    internal void Initialize(UserSkillHandler userSkillHandler)
    {
        if (_userSkillHandler != null)
        {
            _userSkillHandler.OpenedSkillsMenu -= Open;
            _userSkillHandler.ChangedScore -= OnChangeScore;
        }

        _userSkillHandler = userSkillHandler ?? throw new InvalidOperationException("userSkillHandler is null");

        _userSkillHandler.OpenedSkillsMenu += Open;
        _userSkillHandler.ChangedScore += OnChangeScore;
    }

    private void OnChangeScore(int score)
    {
        _scoreText.text = $"{score}";
    }

    private void Open()
    {
        _windowGroup.alpha = 1f;
        _windowGroup.blocksRaycasts = true;
        _exitButton.interactable = true;
        _resetButton.interactable = true;
        UserUtilities.BanRaycast();
    }

    private void Close()
    {
        Time.timeScale = 1;
        _userSkillHandler.ActivateTempScills();

        _windowGroup.alpha = 0f;
        _windowGroup.blocksRaycasts = false;
        _exitButton.interactable = false;
        _resetButton.interactable = false;
        UserUtilities.UnbanRaycast();
    }

    private void OnReset()
    {
        _userSkillHandler.Reset();
    }
}