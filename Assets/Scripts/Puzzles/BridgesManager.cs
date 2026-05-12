using System.Collections.Generic;
using UnityEngine;

public class BridgesManager : MonoBehaviour
{
    [SerializeField] private BridgeButton[] buttons;

    private List<Animator> currentBridges = new List<Animator>();

    public void activateBridges(Animator[] bridges)
    {
        Debug.Log(bridges);
        /*if (currentBridges != null)
        {
            foreach (var bridge in currentBridges)
            {
                bridge.SetBool("Up", false);
            }
        }*/

        if (bridges != null)
        {
            
            currentBridges = new List<Animator>(bridges);
        
            foreach (var bridge in bridges)
            {
                bridge.SetBool("Up", !bridge.GetBool("Up"));
                Debug.Log(bridge.GetBool("Up"));
            }
        }
        else
        {
            currentBridges = null;
        }
    }
}
