using UnityEngine;

public class SlowdownArea : MonoBehaviour
{
    [SerializeField] private float _slowdownAmount = 3f;
    [SerializeField] private string _targerLayerMask = "Enemies";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targerLayerMask))
        {
            ISlowdown enemy = other.gameObject.GetComponent<ISlowdown>();
            enemy.ReceiveSlowdown(_slowdownAmount);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targerLayerMask))
        {
            ISlowdown enemy = other.gameObject.GetComponent<ISlowdown>();
            enemy.ReleaseSlowdown(_slowdownAmount);
        }
    }
}
