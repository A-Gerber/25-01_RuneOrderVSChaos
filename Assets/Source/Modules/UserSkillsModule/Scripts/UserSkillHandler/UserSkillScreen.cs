using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserSkillScreen : Window, IReportableOpenEvent
{
    [SerializeField] private Button _resetButton;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private UserSkillHandler _userSkillHandler;

    public event Action<List<string>> SavedSkills;
    public event Action Opened;

    protected override void OnEnable()
    {
        base.OnEnable();
        _resetButton.onClick.AddListener(() => _userSkillHandler.Reset());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _resetButton.onClick.RemoveListener(() => _userSkillHandler.Reset());
    }

    internal void Initialize(UserSkillHandler userSkillHandler)
    {
        if (_userSkillHandler != null)
        {
            _userSkillHandler.ChangedScore -= OnChangeScore;
        }

        _userSkillHandler = userSkillHandler ?? throw new ArgumentNullException("userSkillHandler is null", nameof(userSkillHandler));

        _userSkillHandler.ChangedScore += OnChangeScore;
    }

    private void OnChangeScore(int score)
    {
        _scoreText.text = $"{score}";
    }


    public override void Close()
    {
        _userSkillHandler.ActivateTempScills();
        SavedSkills?.Invoke(_userSkillHandler.GetSkillsToSave());

        _resetButton.interactable = false;
        base.Close();
    }

    public override void Open()
    {
        Opened?.Invoke();
        _resetButton.interactable = true;
        base.Open();
    }

    protected override void OnExitButtonClick()
    {
        Close();
    }
}
