using System.Collections.Generic;
using UnityEngine;

public class BridgesManager : MonoBehaviour
{
    [SerializeField] private BridgeButton[] buttons;

    private List<GameObject> currentBridges = new List<GameObject>();

    public void activateBridges(GameObject[] bridges)
    {
        Debug.Log(bridges);
        if (currentBridges != null)
        {
            foreach (var bridge in currentBridges)
            {
                bridge.SetActive(false);
            }
        }

        if (bridges != null)
        {
            currentBridges = new List<GameObject>(bridges);
        
            foreach (var bridge in bridges)
            {
                bridge.SetActive(true);
            }
        }
        else
        {
            currentBridges = null;
        }
    }
}
