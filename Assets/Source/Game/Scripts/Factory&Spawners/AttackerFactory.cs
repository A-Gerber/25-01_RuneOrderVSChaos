using UnityEngine;

public class AttackerFactory : MonoBehaviour
{
    [SerializeField] private AttackerPresenter _attackerViewPrefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private int _damagePerProjectile = 1;

    internal AttackerModel Create(int sizeOfLine)
    {
        AttackerModel attacker = new(_damagePerProjectile, sizeOfLine);
        Instantiate(_attackerViewPrefab, _parent).Initialize(attacker);

        return attacker;
    }
}