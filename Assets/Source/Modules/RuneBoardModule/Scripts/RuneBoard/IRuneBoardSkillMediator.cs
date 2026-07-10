using System;

public interface IRuneBoardSkillMediator
{
    public event Action RuneBoardChecking;
    public event Action RuneBoardAttacking;
    public event Action RuneBoardReleasingTargets;

    public int ManaCount { get; }
}