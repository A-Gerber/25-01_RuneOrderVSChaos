using UnityEngine;

public class AttackerFactory : MonoBehaviour
{
    [SerializeField] private AttackerPresenter _attackerPresenterPrefab;
    [SerializeField] private Score—ounterPresenter _score—ounterPresenter;
    [SerializeField] private Transform _parent;
    [SerializeField] private int _numberSimpleCombo = 1;

    internal AttackerModel Create()
    {
        Score—ounter Òounter = new(_numberSimpleCombo);
        Instantiate(_score—ounterPresenter, _parent).Initialize(Òounter);
        AttackerModel attacker = new(Òounter, _numberSimpleCombo);
        Instantiate(_attackerPresenterPrefab, _parent).Initialize(attacker);

        return attacker;
    }
}