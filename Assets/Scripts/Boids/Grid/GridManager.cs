using System;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public struct GridData
{
    public Vector2Int gridSize;
    public float cellSize;
    public float3 min;
    public float3 max;
    public float3 origin;
}

[System.Serializable]
public struct GridCellData
{
    public float3 worldPosition;
    public int2 gridPosition;
}




public class GridManager : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellSize;
    public Vector3 origin;
    public GridData gridData;

    public static GridManager Instance;

    private void Awake()
    {
        Instance = this;

        gridData = new GridData
        {
            gridSize = gridSize,
            cellSize = cellSize,
            origin = origin
        };
        
        SetupGridCells();
        
    }

    private void SetupGridCells()
    {
        for (int x = 0; x < gridData.gridSize.x; x++)
        {
            for (int y = 0; y < gridData.gridSize.y; y++)
            {
                GridCellData data = new GridCellData();

                data.worldPosition = gridData.origin + new float3(
                    x + gridData.cellSize + gridData.cellSize / 2f,
                    0,
                    y + gridData.cellSize + gridData.cellSize / 2f
                    );

                data.gridPosition = new int2(x, y);
            }
        }

        gridData.min = gridData.origin;
        gridData.max = gridData.origin +
                       new float3(gridData.gridSize.x * gridData.cellSize, 0f,
                           gridData.gridSize.y * gridData.cellSize) +
                       new float3(0.0f, 100f, 0.0f);
    }
    
    public static int2 GetGridPos(float3 position, GridData gridData)
    {
        var relativePosition = position - gridData.origin;

        var x = (int)math.clamp((int)(relativePosition.x / gridData.cellSize), 0, gridData.gridSize.x - 1);
        var y = (int)math.clamp((int)(relativePosition.z / gridData.cellSize), 0, gridData.gridSize.y - 1);

        // check if in bounds
        if (x < 0 || x >= gridData.gridSize.x || y < 0 || y >= gridData.gridSize.y)
        {
            return -1;
        }

        return new int2(x, y);
    }

    public static int GetGridIndex(float3 position, GridData gridData)
    {
        var relativePosition = position - gridData.origin;

        var x = (int)math.clamp((int)(relativePosition.x / gridData.cellSize), 0, gridData.gridSize.x - 1);
        var y = (int)math.clamp((int)(relativePosition.z / gridData.cellSize), 0, gridData.gridSize.y - 1);

        // check if in bounds
        if (x < 0 || x >= gridData.gridSize.x || y < 0 || y >= gridData.gridSize.y)
        {
            return -1;
        }

        return x * gridData.gridSize.y + y;
    }

    public static float3 GetWorldPos(int x, int y, GridData gridData)
    {
        return gridData.origin + new float3(x * gridData.cellSize + gridData.cellSize / 2, 0,
            y * gridData.cellSize + gridData.cellSize / 2);
    }

    public static int FlattenGridIndex(int x, int y, GridData gridData)
    {
        return x * gridData.gridSize.y + y;
    }
}
