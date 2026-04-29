using UnityEngine;

public class CinemachineSwitch : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camera1.SetActive(!camera1.activeSelf);
            camera2.SetActive(!camera2.activeSelf);
        }
    }
}
