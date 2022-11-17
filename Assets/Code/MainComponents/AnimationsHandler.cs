using UnityEngine;

public class AnimationsHandler
{
    private Animator _animator;

    public AnimationsHandler(Animator animator)
    {
        SetAnimator(animator);
    }

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void PlayAnimationState(string stateName, float transitionDuration)
    {
        _animator.CrossFade(stateName, transitionDuration);
    }
}
