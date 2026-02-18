using System;
using UnityEngine;
using UnityEngine.UI;

internal class SkillIcon : MonoBehaviour
{
    [SerializeField] private Image _icon;

    internal void SetIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
    }
}