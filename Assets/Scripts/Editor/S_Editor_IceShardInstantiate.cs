using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class IceShards : EditorWindow
{

    public float rayDistance = 10f;
    public Color rayColor = Color.deepSkyBlue;
    public bool showRays = false;
    
    
    [MenuItem("Unite Zero/IceShards")]
    public static void OpenWindow()
    {
        GetWindow<IceShards>();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Show Ice Shards Positions"))
        {
            showRays = true;
        }
    }

    void OnSceneGUI()
    {
        if (showRays)
        {
            foreach (var obj in Selection.objects)
            {
                void OnDrawGizmos()
                {
                    Gizmos.color = rayColor;
                    Gizmos.DrawRay(Selection.activeTransform.position, Selection.activeTransform.forward * rayDistance);
                    Gizmos.DrawSphere(Selection.activeTransform.position + Selection.activeTransform.position.normalized * rayDistance, 0.1f);
                }
            }
        }
    }
}
