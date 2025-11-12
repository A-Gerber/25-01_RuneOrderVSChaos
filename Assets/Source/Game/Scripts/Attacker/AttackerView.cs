using UnityEngine;

namespace RuneOrderVSChaos
{
    public class AttackerView : MonoBehaviour
    {
        private AttackerModel _attackerModel;

        internal void Initialize(AttackerModel attackerModel)
        {
            _attackerModel = attackerModel;
        }
    }
}