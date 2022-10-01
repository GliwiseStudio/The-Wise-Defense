using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateOnMouseClick : MonoBehaviour
{
    public GameObject prefab;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            InstantiateOnPosition(Input.mousePosition);
        }
        
    }

    void InstantiateOnPosition(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out RaycastHit info))
        {
            Debug.Log("Pinchado en " + info.collider.name);
            Vector3 instantiationPoint = info.point;
            GameObject newGO = Instantiate(prefab, instantiationPoint, Quaternion.identity);
            Rigidbody rb = newGO.GetComponent<Rigidbody>();
            if(rb == null)
            {
                rb = newGO.AddComponent<Rigidbody>();
            }

            rb.mass = 10f;
        }
    }
}
