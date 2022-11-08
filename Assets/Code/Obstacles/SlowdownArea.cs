using UnityEngine;
using System.Linq;

public class SlowdownArea : MonoBehaviour
{
    [SerializeField][Range(0.1f, 0.9f)] private float _slowdownPercentage;
    [SerializeField] private string[] _targerLayerMasks;
    [SerializeField] private bool _damageEnemies;

    [Header("If obstacle inflicts damage: ")]
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _time = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (_targerLayerMasks.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            IDownStats enemy = other.gameObject.GetComponent<IDownStats>();
            enemy.ReceiveSlowdown(_slowdownPercentage);
 
            if (_damageEnemies)
            {
                enemy.ReceiveDamageOnLoop(_damage, _time);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_targerLayerMasks.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            IDownStats enemy = other.gameObject.GetComponent<IDownStats>();
            enemy.ReleaseSlowdown(_slowdownPercentage);

            if (_damageEnemies)
            {
                enemy.StopDamageLoop();
            }
        }
    }
}
