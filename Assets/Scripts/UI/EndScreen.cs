using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    private void OnSubmit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCancel()
    {
        Application.Quit();
    }
}
