using System;
using UnityEngine;

public class InvoProtection : MonoBehaviour
{
    [SerializeField] private InvoBehaviour[] _invoBehaviours;
    [SerializeField] private GameObject reference;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AssignNewTarget();
        }
    }


    void AssignNewTarget()
    {
        Vector3 dir = Vector3.zero;

        for (int i = 0; i < _invoBehaviours.Length; i++)
        {
            GameObject go = new GameObject();
            

            if (i == 0)
            {
                dir = Vector3.up;
            }
            
            else if (i < _invoBehaviours.Length / 2)
            {
                dir = Quaternion.Euler(-35,
                    360 / _invoBehaviours.Length * 2 * i, 0) * Vector3.forward;
            }
            else
            {
                dir = Quaternion.Euler(0,
                    360 / _invoBehaviours.Length * 2 *i -  _invoBehaviours.Length / 2 , 0) * Vector3.forward;
            }
            
            go.transform.parent = reference.transform; 
            
            go.transform.localPosition = Vector3.zero;
            
            go.transform.localPosition = dir;

            _invoBehaviours[i].acceleration = 60;
            _invoBehaviours[i].target = go.transform;
        }




    }
    
}
