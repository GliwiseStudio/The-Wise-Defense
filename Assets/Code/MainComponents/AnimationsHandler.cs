using UnityEngine;

public class AnimationsHandler
{
    private readonly Animator _animator;

    public AnimationsHandler(Animator animator)
    {
        _animator = animator;
    }

    public void PlayAnimationState(string stateName, float transitionDuration)
    {
        _animator.CrossFade(stateName, transitionDuration);
    }
}
