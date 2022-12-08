using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBlueprintRange : MonoBehaviour
{
    public void SetRange(float range)
    {
        Transform parent = gameObject.transform.parent;
        gameObject.transform.SetParent(null);
        gameObject.transform.localScale = new Vector3(2 * range, 1, 2 * range); // change rage cylinder back
        gameObject.transform.SetParent(parent);
    }
}
