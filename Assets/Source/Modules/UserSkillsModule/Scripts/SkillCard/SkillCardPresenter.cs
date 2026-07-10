using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCardPresenter : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Toggle _toggle;

    private SkillCard _skillCard;
    private Color _closeColor = new(0.416f, 0.416f, 0.416f);
    private Color _openColor = new(0f, 0.54f, 1f);

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
            _skillCard.ActivatedOnLoad -= OnActivatedOnLoad;
            _skillCard.Opened -= OnOpen;
            _skillCard.Closed -= OnClose;
            _skillCard.ChangedInteractable -= OnChangeInteractable;
        }

        _skillCard = skillCard ?? throw new ArgumentNullException("skillCard is null", nameof(skillCard));

        if (_skillCard != null)
        {
            _skillCard.ActivatedOnLoad += OnActivatedOnLoad;
            _skillCard.Opened += OnOpen;
            _skillCard.Closed += OnClose;
            _skillCard.ChangedInteractable += OnChangeInteractable;
        }

        _icon.sprite = _skillCard.GetIcon();
        _levelText.text = $"<size=35>{_skillCard.OpeningThreshold}<size=22> lv";
        _description.text = _skillCard.GetDescription();
    }

    public void ChangeSkillDescription(Languages language)
    {
        _skillCard.ChangeSkillDescription(language);

        _description.text = _skillCard.GetDescription();
    }

    private void OnActivatedOnLoad()
    {
        if (enabled)
            _toggle.isOn = true;
    }

    private void OnChangeInteractable(bool value)
    {
        if (enabled)
            _toggle.interactable = value;
    }

    private void OnToggleChanged(bool isOn)
    {
        if (!isOn)
            return;

        _toggle.interactable = false;

        try
        {
            _skillCard.Activate();
        }
        catch (Exception ex)
        {
            Debug.Log( $"Непредвиденная ошибка: {ex.Message}");
        }
    }

    private void OnClose()
    {
        if (!enabled)
            return;

        _image.color = _closeColor;
        _canvasGroup.blocksRaycasts = false;
        _toggle.interactable = false;
        _toggle.isOn = false;
    }

    private void OnOpen()
    {
        if (!enabled)
            return;

        _image.color = _openColor;
        _canvasGroup.blocksRaycasts = true;
        _toggle.interactable = true;
    }
}