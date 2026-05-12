using JetBrains.Annotations;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class CinemachineSwitch : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    public CinemachineBrain brain;
    public GameObject[] camerasToActivateOrDeactivate;
    public GameObject[] camerasToDeactivate;
    public GameObject[] camerasToActivate;
    public CinemachineCamera[] camerasToGivePriority;
    public CinemachineCamera[] camerasToRemovePriority;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (camera1 != null && camera2 != null)
            {
                camera1.SetActive(!camera1.activeSelf);
                camera2.SetActive(!camera2.activeSelf);
            }
            
            foreach (GameObject c in camerasToActivateOrDeactivate)
            {
                c.SetActive(!c.activeSelf);
            }
            
            foreach (GameObject c in camerasToActivate)
            {
                c.SetActive(true);
            }
            
            foreach (GameObject c in camerasToDeactivate)
            {
                c.SetActive(false);
            }

            foreach (CinemachineCamera c in camerasToGivePriority)
            {
                c.Priority = 1;
            }
            
            foreach (CinemachineCamera c in camerasToRemovePriority)
            {
                c.Priority = 0;
            }
        }
    }
}
