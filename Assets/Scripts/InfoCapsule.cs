using System;
using TMPro;
using UnityEngine;

public class InfoCapsule : MonoBehaviour
{
    public TextMeshProUGUI infoCanva;

    public string infoText;
    
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            infoCanva.text = infoText;
        }
    }
    
    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            infoCanva.text = "";
        }
    }
}
