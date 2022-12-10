using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconScreenBtn : MonoBehaviour
{
    [SerializeField] private GameObject _iconScreen;
    public void ShowIconScreen()
    {
        _iconScreen.gameObject.SetActive(true);
    }
    public void HideIconScreen()
    {
        _iconScreen.gameObject.SetActive(false);
    }
}
