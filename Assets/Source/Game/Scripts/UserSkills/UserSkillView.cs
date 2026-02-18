using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UserSkillView : MonoBehaviour
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

        _skill = skill ?? throw new InvalidOperationException("skill is null");

        _skill.Used += OnUsed;

        _audioSource.clip = _skill.AudioClip;
    }

    private void OnUsed()
    {
        _audioSource.Play();
    }
}