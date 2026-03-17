using System;
using System.Collections.Generic;
using UnityEngine;

public class InvoProtection : MonoBehaviour
{
    [SerializeField] private GameObject reference;

    private bool isProtecting = false;

    private void OnEnable()
    {
        InvoManager.OnProtection += protect;
    }

    
    private void OnDisable()
    {
        InvoManager.OnProtection -= protect;
    }

    void protect(List<InvoBehaviour> invos)
    {
        if(!isProtecting) NewTarget(25, invos);
        else
        {
            foreach (var invo in invos)
            {
                invo.ChangeState(invo.stateIdle);
            }

            isProtecting = false;
        }
    }

    void NewTarget(float angle, List<InvoBehaviour> invos)
    {

        isProtecting = true;

        float n = 0;
        
        Vector3 dir = Vector3.zero;
        
        List<float> circList = new List<float>();
        
        
        for (int i = 0; i < 5; i++)
        {

            circList.Add(Maths.circleCircumferenceInSphereRadius(angle*(i+1)));
            n += Maths.circleCircumferenceInSphereRadius(angle * (i+1));
        
        }

        int currentIndex = 0;
        int lastIndex = 0;
        
        for (int i = 0; i < invos.Count; i++)
        {
            /*Debug.Log(n * _invoBehaviours.Length / circList[currentIndex]);
            Debug.Log(n);
            Debug.Log( _invoBehaviours.Length);
            Debug.Log(circList[currentIndex]);*/
            
            
            //dir = Quaternion.Euler(-90 / circList.Count * currentIndex,
              //  360 / circList[currentIndex] * i - lastIndex, 0) * Vector3.forward;
            

            Vector3 offset = Maths.coordsCircleInSphere(-angle * circList.Count / circList.Count * currentIndex,
                360 /  (invos.Count * circList[currentIndex] / n) * (i - lastIndex)) * 3;
            
            invos[i]._InvoInstance.offset = offset;
            invos[i].ChangeState(invos[i].stateProtection);
            
            
            if (i - lastIndex > invos.Count * circList[currentIndex] / n)
            {
                currentIndex++;
                lastIndex = i;
            }
        }
    }
}