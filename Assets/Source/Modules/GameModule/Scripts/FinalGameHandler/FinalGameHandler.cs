using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalGameHandler : MonoBehaviour
{
    [SerializeField] private WinGameScreen _winGameScreen;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private EffectConfettiSpawner _effectConfettiSpawner;
    [SerializeField] private TextMeshProUGUI _winnerText;
    [SerializeField] private float _delay = 0.5f;

    private WaitForSeconds _wait;
    private IShowableNextSkills _skillCardDiscoverer;

    public event Action NextLevelButtonClicked;
    public event Action RewardButtonClicked;
    internal event Action RestartButtonClicked;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
    }

    private void OnEnable()
    {
        _winGameScreen.NextLevelButtonClicked += () => 
        { 
            NextLevelButtonClicked?.Invoke();
            _winGameScreen.Close();
        };

        _endGameScreen.RestartButtonClicked += () =>
        {
            RestartButtonClicked?.Invoke();
            _endGameScreen.Close();
        };

        _endGameScreen.RewardButtonClicked += () =>
        {
            RewardButtonClicked?.Invoke();
            _endGameScreen.Close();
        };
    }

    private void OnDisable()
    {
        _winGameScreen.NextLevelButtonClicked -= () =>
        {
            NextLevelButtonClicked?.Invoke();
            _winGameScreen.Close();
        };

        _endGameScreen.RestartButtonClicked -= () =>
        {
            RestartButtonClicked?.Invoke();
            _endGameScreen.Close();
        };

        _endGameScreen.RewardButtonClicked -= () =>
        {
            RewardButtonClicked?.Invoke();
            _endGameScreen.Close();
        };
    }

    internal void Initialize (IShowableNextSkills skillCardDiscoverer)
    {
        _skillCardDiscoverer = skillCardDiscoverer ?? throw new ArgumentNullException("skillCardDiscoverer is null", nameof(skillCardDiscoverer));
    }

    internal void Win(int level, int scoreIncrease, int manaCountIncrease)
    {
        if (_skillCardDiscoverer.TryGetSkillSprites(out List<Sprite> sprites, level))
            _winGameScreen.ShowOpenSkills(sprites);
        else
            _winGameScreen.HideSkills(_skillCardDiscoverer.GetNextThreshold(level), level);

        if (level - 1 == Constants.LastLevel)
            _winGameScreen.ShowWitch();
        else
            _winGameScreen.HideWitch();

        _effectConfettiSpawner.CreateEffect();
        _winGameScreen.UpdateIncreases(scoreIncrease, level, manaCountIncrease);
        StartCoroutine(OpenWinScreenOverTime());
    }

    private IEnumerator OpenWinScreenOverTime()
    {
        _winnerText.gameObject.SetActive(true);
        yield return _wait;
        yield return _wait;
        _winnerText.gameObject.SetActive(false);
        _winGameScreen.Open();
    }

    internal void Finish(int manaIncrease)
    {
        StartCoroutine(OpenEndScreenOverTime(manaIncrease));
    }

    private IEnumerator OpenEndScreenOverTime(int manaIncrease)
    {
        _endGameScreen.ChangeManaIncrease(manaIncrease);
        _endGameScreen.ShowWitch();
        yield return _wait;
        _endGameScreen.Open();
    }
}