using UnityEngine;

namespace RuneOrderVSChaos
{
    public class AttackerFactory : MonoBehaviour
    {
        [SerializeField] private AttackerView _attackerViewPrefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private int _damagePerProjectile = 1;

        internal AttackerModel Create()
        {
            AttackerModel attacker = new(_damagePerProjectile);

            Instantiate(_attackerViewPrefab, _parent).Initialize(attacker);

            return attacker;
        }
    }
}