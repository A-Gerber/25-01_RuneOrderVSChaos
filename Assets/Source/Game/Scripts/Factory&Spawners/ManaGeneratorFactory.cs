using UnityEngine;

public class ManaGeneratorFactory : MonoBehaviour
{
    [SerializeField] private ManaGeneratorView _manaGeneratorViewPrefab;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private int _manaPerCube = 1;

    internal ManaGenerator Create(int minSkillCost)
    {
        ManaGenerator manaGenerator = new(_manaPerCube, minSkillCost);
        Instantiate(_manaGeneratorViewPrefab, _parent).Initialize(manaGenerator);

        return manaGenerator;
    }
}