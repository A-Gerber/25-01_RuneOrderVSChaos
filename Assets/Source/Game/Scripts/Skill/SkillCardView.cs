using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class SkillCardView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Toggle _toggle;

    private SkillCard _skillCard;
    private Color _closeColor = new(0.416f, 0.416f, 0.416f);
    private Color _openColor = new(0f,0.54f,1f);

    private void OnEnable()
    {
        _toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    public void Initialize(SkillCard skillCard)
    {      
        if (_skillCard != null)
        {
            _skillCard.Opened -= OnOpen;
            _skillCard.Closed -= OnClose;
            _skillCard.ChangedInteractable -= OnChangeInteractable;
        }

        _skillCard = skillCard ?? throw new InvalidOperationException("skillCard is null");

        _skillCard.Opened += OnOpen;
        _skillCard.Closed += OnClose;
        _skillCard.ChangedInteractable += OnChangeInteractable;

        _icon.sprite = _skillCard.GetIcon();
        _levelText.text = _skillCard.OpeningThreshold.ToString();
        _description.text = _skillCard.GetDescription();
    }

    private void OnChangeInteractable(bool value)
    {
        _toggle.interactable = value;
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            _skillCard.Activate();
            _toggle.interactable = false;
        }
    }

    private void OnClose()
    {
        _image.color = _closeColor;
        _canvasGroup.blocksRaycasts = false;
        _toggle.interactable = false;
        _toggle.isOn = false;
    }

    private void OnOpen()
    {
        _image.color = _openColor;
        _canvasGroup.blocksRaycasts = true;
        _toggle.interactable = true;
    }
}
