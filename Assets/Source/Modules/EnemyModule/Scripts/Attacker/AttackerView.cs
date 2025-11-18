using UnityEngine;

public class AttackerView : MonoBehaviour
{
    private AttackerModel _attackerModel;

    public void Initialize(AttackerModel attackerModel)
    {
        _attackerModel = attackerModel;
    }
}