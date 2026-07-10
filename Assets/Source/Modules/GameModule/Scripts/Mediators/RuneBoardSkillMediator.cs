using System;
using UnityEngine;

internal class RuneBoardSkillMediator : MonoBehaviour, IRuneBoardSkillMediator
{
    private ISkillRuneBoardContactable _userSkillPerformer;

    public event Action RuneBoardChecking;
    public event Action RuneBoardAttacking;
    public event Action RuneBoardReleasingTargets;

    public int ManaCount => _userSkillPerformer.ManaCount;

    internal void Initialize(ISkillRuneBoardContactable userSkillPerformer)
    {
        if (_userSkillPerformer != null)
        {
            _userSkillPerformer.RuneBoardChecking -=() => RuneBoardChecking?.Invoke();
            _userSkillPerformer.RuneBoardAttacking -= () => RuneBoardAttacking?.Invoke();
            _userSkillPerformer.RuneBoardReleasingTargets -= () => RuneBoardReleasingTargets?.Invoke();
        }

        _userSkillPerformer = userSkillPerformer ?? throw new ArgumentNullException("userSkillPerformer is null", nameof(userSkillPerformer));

        if (_userSkillPerformer != null)
        {
            _userSkillPerformer.RuneBoardChecking += () => RuneBoardChecking?.Invoke();
            _userSkillPerformer.RuneBoardAttacking += () => RuneBoardAttacking?.Invoke();
            _userSkillPerformer.RuneBoardReleasingTargets += () => RuneBoardReleasingTargets?.Invoke();
        }
    }
}