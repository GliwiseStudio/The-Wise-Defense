using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    private Button button;
    public CardConfigurationSO cardConfig;

    public void Activate(Transform transform)
    {
        cardConfig.cardPower.Power.Activate(transform);
    }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private Transform GetTransform()
    {
        return transform;
    }

    private void OnEnable(Transform transform)
    {
        button.onClick.AddListener(ActivateOnMouseClick);
        //Transform tr = GetTransform();
        //button.onClick.AddListener(Activate(tr));
    }

    private void OnDissable()
    {
        button.onClick.RemoveListener(ActivateOnMouseClick);
    }

    public void ActivateOnMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit info, 100000, LayerMask.GetMask("Enemies")))
        {
            //Debug.Log("Pinchado en " + info.collider.name);

            GameObject newGO = new GameObject();
            newGO.transform.position = info.point;

            Activate(newGO.transform);
        }
    }
}
