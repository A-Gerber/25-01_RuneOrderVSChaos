using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UserSkillPresenter : MonoBehaviour
{
    private UserSkill _skill;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(UserSkill skill)
    {
        if (_skill != null)
            _skill.Used -= OnUsed;

        _skill = skill ?? throw new ArgumentNullException("skill is null", nameof(skill));

        if (_skill != null)
            _skill.Used += OnUsed;

        _audioSource.clip = _skill.AudioClip;
    }

    private void OnUsed()
    {
        if (enabled)
            _audioSource.Play();
    }
}