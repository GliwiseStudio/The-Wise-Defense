using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ProvisionalGanarOPerder : MonoBehaviour
{
    private GameManager _gameManager;
    public TextMeshProUGUI text;
    public GameObject boton;
    private void Awake()
    {
        _gameManager = GameManager.Instance;
        boton.SetActive(false);
    }

    private void OnEnable()
    {
        _gameManager.OnWin += Win;
        _gameManager.OnLoose += Loose;
    }

    private void OnDisable()
    {
        _gameManager.OnWin -= Win;
        _gameManager.OnLoose -= Loose;
    }

    private void Win()
    {
        boton.SetActive(true);
        text.gameObject.SetActive(true);
        text.text = "¡Has ganado! Pulsa aquí para volver al menú";
    }

    private void Loose()
    {
        boton.SetActive(true);
        text.gameObject.SetActive(true);
        text.text = "¡Has perdido! Pulsa aquí para volver al menú";
    }

    public void CargarMenu()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
