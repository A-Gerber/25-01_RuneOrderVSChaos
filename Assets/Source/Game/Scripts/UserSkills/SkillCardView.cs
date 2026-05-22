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

        Subscribe();
    }

    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveListener(OnToggleChanged);

        Unsubscribe();
    }

    public void Initialize(SkillCard skillCard)
    {
        Unsubscribe();

        _skillCard = skillCard ?? throw new InvalidOperationException("skillCard is null");

        Subscribe();

        _icon.sprite = _skillCard.GetIcon();
        _levelText.text = $"<size=35>{_skillCard.OpeningThreshold}<size=22> lv";
        _description.text = _skillCard.GetDescription();
    }

    internal void ChangeSkillDescription(Languages language)
    {
        _skillCard.ChangeSkillDescription(language);

        _description.text = _skillCard.GetDescription();
    }

    private void OnActivatedOnLoad()
    {
        _toggle.isOn = true;
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

    private void Subscribe()
    {
        if (_skillCard != null)
        {
            _skillCard.ActivatedOnLoad += OnActivatedOnLoad;
            _skillCard.Opened += OnOpen;
            _skillCard.Closed += OnClose;
            _skillCard.ChangedInteractable += OnChangeInteractable;
        }
    }

    private void Unsubscribe()
    {
        if (_skillCard != null)
        {
            _skillCard.ActivatedOnLoad -= OnActivatedOnLoad;
            _skillCard.Opened -= OnOpen;
            _skillCard.Closed -= OnClose;
            _skillCard.ChangedInteractable -= OnChangeInteractable;
        }
    }
}
