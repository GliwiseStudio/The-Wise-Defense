using UnityEngine;

public class TowerAnimationsHandler
{
    private readonly Animator _animator;

    public TowerAnimationsHandler(Animator animator)
    {
        _animator = animator;
    }

    public void PlayAnimationState(string stateName, float transitionDuration)
    {
        _animator.CrossFade(stateName, transitionDuration);
    }
}
