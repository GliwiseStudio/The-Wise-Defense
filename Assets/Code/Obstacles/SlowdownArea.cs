using UnityEngine;
using System.Linq;

public class SlowdownArea : MonoBehaviour
{
    [SerializeField][Range(0.1f, 0.9f)] private float _slowdownPercentage;
    [SerializeField] private string[] _targerLayerMasks;

    private void OnTriggerEnter(Collider other)
    {
        if (_targerLayerMasks.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            IDownStats enemy = other.gameObject.GetComponent<IDownStats>();
            enemy.ReceiveSlowdown(_slowdownPercentage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_targerLayerMasks.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            IDownStats enemy = other.gameObject.GetComponent<IDownStats>();
            enemy.ReleaseSlowdown(_slowdownPercentage);
        }
    }
}
