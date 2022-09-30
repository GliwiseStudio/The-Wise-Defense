using UnityEngine;

public abstract class ProjectileDamagerSO : ScriptableObject
{
    protected abstract IProjectileDamager InitializeDamager();
    public IProjectileDamager Damager => InitializeDamager();
}
