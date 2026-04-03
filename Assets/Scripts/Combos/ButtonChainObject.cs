using System.Collections.Generic;
using UnityEngine;

public class ButtonChainObject
{
    public List<direction> Directions;
    
    public enum direction
    {
        north,
        east,
        south,
        west
    }

    public ButtonChainObject()
    {
        Directions = new List<direction>();
    }
}
