using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Projectile/Damage Type/AreaDamage", fileName = "ProjectileAreaDamageConfiguration")]
public class ProjectileAreaDamagerSO : ProjectileDamagerSO
{
    [SerializeField] private float _radius;
    [SerializeField] private string _targetLayerMask;
    protected override IProjectileDamager InitializeDamager()
    {
        return new ProjectileAreaDamager(_radius, _targetLayerMask);
    }
}
