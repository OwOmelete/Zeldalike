using UnityEngine;

public class pcafterEnd : MonoBehaviour
{
    public GameObject canvas;
    public GameObject Y;
    bool canAccess;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other);
            ReferenceUi refUi = other.gameObject.GetComponent<ReferenceUi>();

            if(refUi == null)
            {
                Debug.Log("ReferenceUi introuvable !");
                return;
            }

            canvas = refUi.Reference();
        }
    }
    void PcAfterEnter()
    {
        if (CinematiqueManager.INSTANCE.ActualAnim == 4)
        {
            canvas.SetActive(true);
        }
    }
    
}
