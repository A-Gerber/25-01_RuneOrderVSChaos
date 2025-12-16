using UnityEngine;

public class AttackerFactory : MonoBehaviour
{
    [SerializeField] private AttackerPresenter _attackerViewPrefab;
    [SerializeField] private Transform _parent;

    internal AttackerModel Create(int sizeOfLine)
    {
        AttackerModel attacker = new(sizeOfLine);
        Instantiate(_attackerViewPrefab, _parent).Initialize(attacker);

        return attacker;
    }
}