using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float stopDistance;
    [SerializeField] private float maxDistance;
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
        
        float distance = Vector3.Distance(rb.position, transform.position);
        
        if (other.gameObject.tag == "ActiveAutomate" && toggleMagnet)
        {

            if (distance >= stopDistance)
            {
                float normalizedDistance = Mathf.Clamp01(distance / maxDistance);
                float pull = force * normalizedDistance * normalizedDistance;
                
                Vector3 direction = (transform.position - rb.position).normalized;

                rb.AddForce(direction * pull, ForceMode.Acceleration);

            }
        }
    }
}
