using System;
using UnityEngine;

public class InvoBehaviour : MonoBehaviour
{
    public InvoData _Invo;
    public InvoDataInstance _InvoInstance;
    public Transform player;
    [SerializeField] private Rigidbody rb;

    public static event Action<InvoDataInstance> OnInvoSpawn; 
    
    private void Start()
    {
        _InvoInstance = _Invo.Instance();
        
        OnInvoSpawn?.Invoke(_InvoInstance);
        
        _InvoInstance.rb = rb;

        _InvoInstance.currentState = InvoData.State.idle;
    }

    private void FixedUpdate()
    {
        Movement();
    }
    
    void Movement()
    {
        /*if ((transform.position - target.position).magnitude > maxDistance)
        {
            rb.AddForce(getDirection().normalized * acceleration);
            return;
        }*/
        
        //^^
        if (_InvoInstance.rb.linearVelocity.magnitude > _InvoInstance.maxSpeed)
        {
            Vector3 force = Maths.OrthogonalProjection(
                getDirection().normalized * _InvoInstance.acceleration,
                _InvoInstance.rb.linearVelocity.normalized * _InvoInstance.maxSpeed);
            _InvoInstance.rb.AddForce(force);
        }
        
        else
        {
            _InvoInstance.rb.AddForce(getDirection().normalized * _InvoInstance.acceleration);
        }
    }

    void Protection()
    {
        
    }
    
    Vector3 getDirection()
    {
        if (_InvoInstance.currentState == InvoData.State.idle)
        {
            //Maths.idleOffset(3, 3f)
            Vector3 dir = player.position + _InvoInstance.offset  - transform.position;
            return new Vector3(dir.x, 0, dir.z);
        }
        return player.position + _InvoInstance.offset - transform.position;
    }
}
