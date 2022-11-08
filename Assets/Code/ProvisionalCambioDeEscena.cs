using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class ProvisionalCambioDeEscena : MonoBehaviour
{
    public void CambiarEscenaProvisional()
    {
        SceneManager.LoadScene("PickLevel", LoadSceneMode.Single);
    }
}
