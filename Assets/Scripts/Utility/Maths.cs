using UnityEngine;

public class Maths : MonoBehaviour
{
    public static Vector3 OrthogonalProjection(Vector3 v, Vector3 u)
    {
        float dot = Vector3.Dot(v, u);
        
        Vector3 proj = (dot / Vector3.Dot(u, u)) * u;
        
        if (dot > 0)
        {
            return v - proj;
        }
        return v + proj;
    }
}
