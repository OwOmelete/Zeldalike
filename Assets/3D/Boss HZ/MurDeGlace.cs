using UnityEngine;

public class MurDeGlace : MonoBehaviour
{
    public InvoManager invoManager;
    public int NombreFamillierRequis;
    public GameObject Mur;

    void OnTriggerEnter(Collider other)
    {
        int count = invoManager.InvoList.Count;
        if(count > NombreFamillierRequis)
        {
            Destroy(Mur);
        }
    }
}
