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
        return Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Cos(t%360 * Mathf.Deg2Rad);
    }
    
    public static float yCircleInSphere(float angle, float t)
    {
        return Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Sin(t%360* Mathf.Deg2Rad);
    }
    
    public static float zCircleInSphere(float angle, float t)
    {
        return Mathf.Cos(angle * Mathf.Deg2Rad);
    }

    public static Vector3 coordsCircleInSphere(float angle, float t)
    {
        return new Vector3(xCircleInSphere(angle, t), zCircleInSphere(angle, t), yCircleInSphere(angle, t));
    }
    
    public static float circleCircumferenceInSphereRadius(float angle)
    {
        //Debug.Log(Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad)));
        return Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad ));
    }
    
    public static Vector3 idleOffset(float t, float d)
    {
        return new Vector3(
            Mathf.Cos(Time.time * t*0.7f)*d*1.5f,
            Mathf.Sin(Time.time * t*0.3f)*d*0.3f,
            Mathf.Sin(Time.time * t*0.7f)*d*1.5f);
    }
}