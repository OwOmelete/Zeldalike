using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Unity.Jobs;

public class UnitBoidSystem
{
    public void Execute(ref NativeArray<UnitBoidData> inputBoids,
        ref NativeParallelMultiHashMap<int, UnitBoidData> hashMap, bool clumpNearTarget, float avoidanceForce)
    {
        float deltaTime = math.min(0.05f, Time.deltaTime);
        var r = new Unity.Mathematics.Random((uint)Time.frameCount);

        var avoidanceCalculationJob = new UnitAvoidanceCalculationJob
        {
            random = r,
            DeltaTime = deltaTime,
            HashedUnits = hashMap,
            boids = inputBoids,
            data = GridManager.Instance.gridData,
            AvoidanceForce = avoidanceForce
        };

        var moveAlongHeadingJob = new UnitMoveAlongHeadingJob
        {
            DeltaTime = deltaTime,
            Boids = inputBoids,
            ClumpNearTarget = clumpNearTarget
        };

        if (Time.frameCount % 2 == 0)
        {
            var a = avoidanceCalculationJob.Schedule(inputBoids.Length, 32);
            var b = moveAlongHeadingJob.Schedule(inputBoids.Length, 32, a);
            
            b.Complete();
        }
        else
        {
            var b = moveAlongHeadingJob.Schedule(inputBoids.Length, 32);
            
            b.Complete();
        }
    }
    
}
