using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public Door[] doors;
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!activated)
            {
                activated = true;
                foreach (var door in doors)
                {
                    door.Close();
                }
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            Debug.Log("PlayerDetected");
        }
    }
}
