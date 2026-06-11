using Unity.Mathematics;
using UnityEngine;

public class noteData : MonoBehaviour
{
    public int piste;
    public Vector3 speedLateral;
    public Vector3 decalageLKateral;
    public Vector3 quaternion;
    void Start()
    {
        switch (piste)
        {
            case 1:
    transform.position += decalageLKateral;
    transform.localRotation = Quaternion.Euler(0, 0, -6);
    break;

case 2:
    transform.position += 0.5f * decalageLKateral;
    transform.localRotation = Quaternion.Euler(0, 0 , -3);
    break;

case 3:
    transform.position -= 0.5f * decalageLKateral;
    transform.localRotation = Quaternion.Euler(0, 0,3);
    break;

case 4:
    transform.position -= decalageLKateral;
    transform.localRotation = Quaternion.Euler(0, 0, 6);
    break;
        }
    }
    void Update()
    {
        switch (piste)
        {
            case 1:
            transform.position += speedLateral*Time.deltaTime; 
            break;
             case 2:
            transform.position += 0.5f * speedLateral*Time.deltaTime; 
            break;
             case 3:
            transform.position -= 0.5f * speedLateral*Time.deltaTime; 
            break;
             case 4:
            transform.position -= speedLateral*Time.deltaTime; 
            break;
        }
    }
}
