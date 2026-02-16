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

    public static float xCircleInSphere(float angle, float t)
    {
        return Mathf.Sin(angle) * Mathf.Cos(t);
    }
    
    public static float yCircleInSphere(float angle, float t)
    {
        return Mathf.Sin(angle) * Mathf.Sin(t);
    }
    
    public static float zCircleInSphere(float angle, float t)
    {
        return Mathf.Cos(angle);
    }

    public static float circleCircumferenceInSphereRadius(float angle)
    {
        return Mathf.Sin(angle);
    }
}
