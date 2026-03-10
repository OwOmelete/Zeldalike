using UnityEngine;

public class Pillier_Interaction : MonoBehaviour
{
    public float NBFamillier;

    void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Ajoue de "+NBFamillier+" Famillier");
            //Interact
        }
    }
}
