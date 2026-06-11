using UnityEngine;

public class noteData : MonoBehaviour
{
    public int piste;
    public Vector3 speedLateral;

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
