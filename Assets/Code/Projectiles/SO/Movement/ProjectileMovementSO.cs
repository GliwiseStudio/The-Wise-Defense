using UnityEngine;

public abstract class ProjectileMovementSO : ScriptableObject
{
    protected abstract IProjectileMovement InitializeMovement();
    public IProjectileMovement Movement()
    {
        return InitializeMovement();
    }
}
