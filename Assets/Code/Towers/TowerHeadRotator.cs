using UnityEngine;

public class TowerHeadRotator
{
    private readonly Transform _headTransform;

    public TowerHeadRotator(Transform headTransform)
    {
        _headTransform = headTransform;
    }

    public void Update(Transform target)
    {
        FollowTarget(target);
    }
    
    private void FollowTarget(Transform target)
    {
        _headTransform.LookAt(target, _headTransform.up);
    }
}
