using System;
using UnityEngine;

public class StateVisualizer : MonoBehaviour
{
    [SerializeField] private Material enemyMaterial;

    [SerializeField] private BasicEnemyV2 basicEnemyV2ScriptReference;

    private void Update()
    {
        if (basicEnemyV2ScriptReference.currentStateFlag == BasicEnemyV2.StateFlags.IDLE)
        {enemyMaterial.color = Color.green;}
        else if (basicEnemyV2ScriptReference.currentStateFlag == BasicEnemyV2.StateFlags.CHASING)
        {enemyMaterial.color = Color.blue;}
        else if (basicEnemyV2ScriptReference.currentStateFlag == BasicEnemyV2.StateFlags.ATTACKING)
        {enemyMaterial.color = Color.red;}
    }
}
