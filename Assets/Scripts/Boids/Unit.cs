using Unity.Mathematics;
using UnityEngine;


[System.Serializable]
public struct UnitBoidData
{
    public int id;
    public float3 position;
    public float speedFactor;
    public float predictedSpeedFactor;
    public float3 avoidanceHeading;
    public float3 target;
    public bool hasTarget;
    public float3 nextPosition;
    public float movementSpeed;
    public float rotationalSpeed;
    public float boidSize;
    public quaternion rotation;
}

public class Unit : MonoBehaviour
{
    public static int IDCounter = 0;

    public UnitBoidData boidData;

    private void Awake()
    {
        boidData.id = IDCounter++;
    }

    public void ApplyDataToBoid()
    {
        boidData.position = transform.position;
        boidData.rotation = transform.rotation;
    }

    public void ApplyBoidData(UnitBoidData newData)
    {
        transform.position = newData.nextPosition;
        transform.rotation = newData.rotation;
        boidData.speedFactor = newData.speedFactor;
        boidData.avoidanceHeading = newData.avoidanceHeading;
        boidData.predictedSpeedFactor = newData.predictedSpeedFactor;
        boidData.position = transform.position;
        boidData.rotation = transform.rotation;
    }

    public void MoveTo(float x, float y)
    {
        boidData.target = new float3(x, 0f, y);
        boidData.hasTarget = true;
    }

    public void Stop()
    {
        boidData.hasTarget = false;
    }
}
