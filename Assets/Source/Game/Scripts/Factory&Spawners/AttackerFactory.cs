using UnityEngine;

public class AttackerFactory : MonoBehaviour
{
    [SerializeField] private AttackerView _attackerPresenter;
    [SerializeField] private Score—ounterView _score—ounterPresenter;
    [SerializeField] private int _numberSimpleCombo = 1;

    internal AttackerModel Create()
    {
        ScoreCounter Òounter = new(_numberSimpleCombo);
        AttackerModel attacker = new(Òounter, _numberSimpleCombo);
        _score—ounterPresenter.Initialize(Òounter);
        _attackerPresenter.Initialize(attacker);

        return attacker;
    }
}