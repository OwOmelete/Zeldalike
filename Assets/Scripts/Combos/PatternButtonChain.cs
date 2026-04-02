using UnityEngine;


[CreateAssetMenu(fileName = "patternButtonChain")]
public class patternButtonChain : ScriptableObject
{
    public string name;
    
    public ButtonChainObject.direction[] Directions;
}