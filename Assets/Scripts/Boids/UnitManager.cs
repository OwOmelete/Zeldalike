using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [Header("Movement options")] public bool moveToClick = false;
    public bool clumpAroundTarget = false;
    public float avoidanceForce = 20f;

    private UnitBoidSystem _unitBoidSystem = new UnitBoidSystem();
    
    [HideInInspector] public List<Unit> units = new List<Unit>();

    private bool isSimulationStarted = false;
    public static UnitManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        units = new List<Unit>(GameObject.FindObjectsOfType<Unit>());
        yield return new WaitForEndOfFrame();
        isSimulationStarted = true;
    }

    private Vector3 target = Vector3.zero;

    private void Update()
    {
        if (isSimulationStarted)
        {
            if (Input.GetMouseButtonDown(0) && moveToClick)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    target = hit.point;
                }

                for (int i = 0; i < units.Count; i++)
                {
                    units[i].MoveTo(target.x, target.z);
                }
            }

            NativeArray<UnitBoidData> boids = new NativeArray<UnitBoidData>(units.Count, Allocator.TempJob);
            var hashMap = new NativeParallelMultiHashMap<int, UnitBoidData>(
                GridManager.Instance.gridData.gridSize.x * GridManager.Instance.gridData.gridSize.y,
                Allocator.TempJob);
            
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i] != null)
                {
                    units[i].ApplyDataToBoid();
                    boids[i] = units[i].boidData;

                    var data = units[i].boidData;
                    int gridIndex = GridManager.GetGridIndex(data.position, GridManager.Instance.gridData);
                    
                    hashMap.Add(gridIndex, boids[i]);
                }
                else
                {
                    boids[i] = new UnitBoidData();
                }
            }
            
            _unitBoidSystem.Execute(ref boids,ref hashMap, clumpAroundTarget, avoidanceForce);
            

            for (int i = 0; i < units.Count; i++)
            {
                units[i].ApplyBoidData(boids[i]);
            }

            boids.Dispose();
            hashMap.Dispose();

        }

        
    }
}
