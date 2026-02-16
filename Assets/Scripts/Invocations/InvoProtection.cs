using System;
using System.Collections.Generic;
using UnityEngine;

public class InvoProtection : MonoBehaviour
{
    [SerializeField] private InvoBehaviour[] _invoBehaviours;
    [SerializeField] private GameObject reference;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewTarget(25);
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

    void NewTarget(float angle)
    {

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
        
        for (int i = 0; i < _invoBehaviours.Length; i++)
        {
            GameObject go = new GameObject();
            
            /*Debug.Log(n * _invoBehaviours.Length / circList[currentIndex]);
            Debug.Log(n);
            Debug.Log( _invoBehaviours.Length);
            Debug.Log(circList[currentIndex]);*/
            
            
            //dir = Quaternion.Euler(-90 / circList.Count * currentIndex,
              //  360 / circList[currentIndex] * i - lastIndex, 0) * Vector3.forward;
            
            
            go.transform.parent = reference.transform; 
            
            go.transform.localPosition = Vector3.zero;
            
            Debug.Log(360 /  _invoBehaviours.Length * circList[currentIndex] / n * (i - lastIndex));

            go.transform.localPosition = Maths.coordsCircleInSphere(-angle * circList.Count / circList.Count * currentIndex,
                360 /  (_invoBehaviours.Length * circList[currentIndex] / n) * (i - lastIndex));
            
            _invoBehaviours[i].acceleration = 60;
            _invoBehaviours[i].target = go.transform;
            
            if (i - lastIndex > _invoBehaviours.Length * circList[currentIndex] / n)
            {
                Debug.Log("hihi");
                currentIndex++;
                lastIndex = i;
            }
        }
    }
}