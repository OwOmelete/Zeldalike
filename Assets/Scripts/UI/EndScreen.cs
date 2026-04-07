using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    private void OnSubmit()
    {
        Debug.Log("coucou");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCancel()
    {
        Application.Quit();
    }
}
