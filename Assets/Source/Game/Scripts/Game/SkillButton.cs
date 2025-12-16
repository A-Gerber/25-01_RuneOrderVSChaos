using System;
using UnityEngine;
using UnityEngine.UI;

internal class SkillButton : MonoBehaviour
{
    [SerializeField] private Button _skillButton;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _image;

    private Sprite _startIcon;
    private Color _closeColor;
    private Color _openColor = Color.white;
    private UserSkill _skill;

    internal event Action<UserSkill> ButtonClicked;

    private void Awake()
    {
        _closeColor = _image.color;
        _startIcon = _icon.sprite;
        ResetButton();
    }

    private void OnEnable()
    {
        _skillButton.onClick.AddListener(OnSkillButtonClick);
    }

    private void OnDisable()
    {
        _skillButton.onClick.RemoveListener(OnSkillButtonClick);
    }

    internal void ResetButton()
    {
        _icon.sprite = _startIcon;
        _image.color = _closeColor;
        _skillButton.interactable = false;
    }

    internal void SetUserSkill(UserSkill skill)
    {
        _skill = skill ?? throw new InvalidOperationException("skill is null");
        _icon.sprite = skill.IconOnButton;
        _image.color = _openColor;
        _skillButton.interactable = true;
    }

    private void OnSkillButtonClick()
    {
        ButtonClicked?.Invoke(_skill);
    }
}
