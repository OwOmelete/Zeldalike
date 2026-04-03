using UnityEngine;

public class TestInputs : MonoBehaviour
{
    void OnPauseMenu()
    {
        Debug.Log("PauseMenuInput pressed");
        GlobalEvents.PauseButtonPressed();
    }
}
