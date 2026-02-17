using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float stopDistance;
    [SerializeField] private bool toggleMagnet;
    
    public bool OutsideMagnetToggle
    {
        get => toggleMagnet;
        set => toggleMagnet = value;
    }
    
    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        if (other.gameObject.tag == "Automates" && toggleMagnet)
        {
            float currentDistance = Vector3.Distance(other.transform.position, gameObject.transform.position);

            if (currentDistance > stopDistance)
            {
                float pull = (currentDistance - stopDistance) * force;
                Vector3 direction = (transform.position - rb.position).normalized;

                rb.linearVelocity += direction * pull * Time.fixedDeltaTime;
            }
        }
    }
}
