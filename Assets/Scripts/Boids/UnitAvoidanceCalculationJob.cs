using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[BurstCompile]
public struct UnitAvoidanceCalculationJob : IJobParallelFor
{
    public float DeltaTime;
    public float AvoidanceForce;
    [ReadOnly] public Random random;

    [ReadOnly] public NativeParallelMultiHashMap<int, UnitBoidData> HashedUnits;
    public NativeArray<UnitBoidData> boids;
    [ReadOnly] public GridData data;
    
    [BurstCompile]
    public void Execute(int index)
    {
        UnitBoidData currentBoid = boids[index];
        float3 boidAvoidanceDirection = float3.zero;
        float avgNeighboursGoingToSamePlaceSpeedFactor = currentBoid.predictedSpeedFactor;
        float myDistanceToTarget = math.distance(currentBoid.target, currentBoid.position);

        float3 zeroObst = random.NextFloat3Direction();
        zeroObst.y = 0f;

        var center = GridManager.GetGridPos(currentBoid.position, data);

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                var neighbour = center + new int2(x, y);
                int neighbourIndex = GridManager.FlattenGridIndex(neighbour.x, neighbour.y, data);

                if (HashedUnits.TryGetFirstValue(neighbourIndex, out UnitBoidData neighbourBoid, out var iterator))
                {
                    do
                    {
                        int entity = neighbourBoid.id;

                        if (entity != currentBoid.id)
                        {
                            float3 translation = neighbourBoid.position;
                            float3 obstacleOffset = translation - currentBoid.position;

                            float distanceToObstacle = math.length(obstacleOffset);
                            float otherBoidRadius = neighbourBoid.boidSize;

                            if (distanceToObstacle < otherBoidRadius + currentBoid.boidSize)
                            {
                                obstacleOffset.y = 0f;
                                float avoidancePerc = math.clamp(otherBoidRadius - distanceToObstacle, 0f, 1f);

                                float3 avoidance = math.normalizesafe(obstacleOffset);

                                if (math.length(obstacleOffset) == 0)
                                {
                                    boidAvoidanceDirection -= zeroObst + avoidancePerc;
                                }
                                else
                                {
                                    float sizeFactor = math.clamp(currentBoid.boidSize / otherBoidRadius,
                                        0.04f, 10f);

                                    boidAvoidanceDirection -= avoidance * avoidancePerc * sizeFactor;
                                }
                            }
                        }
                    } while (HashedUnits.TryGetNextValue(out neighbourBoid, ref iterator));
                }


            }
        }
        boidAvoidanceDirection.y = 0f;

        currentBoid.avoidanceHeading = boidAvoidanceDirection * AvoidanceForce;

        if (currentBoid.hasTarget)
        {
            float speedFactor = math.lerp(currentBoid.speedFactor, avgNeighboursGoingToSamePlaceSpeedFactor,
                DeltaTime * 15f);

            currentBoid.speedFactor = speedFactor;
        }
        boids[index] = currentBoid;
    }
}
