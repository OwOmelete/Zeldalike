using System;
using UnityEngine;

public class InvoProtection : MonoBehaviour
{
    [SerializeField] private InvoBehaviour[] _invoBehaviours;
    [SerializeField] private Transform reference;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AssignNewTarget();
        }
    }


    void AssignNewTarget()
    {
        Vector3 dir = Vector3.forward;

        for (int i = 0; i < _invoBehaviours.Length; i++)
        {

            dir = Quaternion.Euler(0, 360 / _invoBehaviours.Length, 0) * dir;

            GameObject go = new GameObject();

            go.transform.parent = reference;

            go.transform.position = dir;

            _invoBehaviours[i].acceleration = 60;
            _invoBehaviours[i].target = go.transform;
        }




    }
    
}
