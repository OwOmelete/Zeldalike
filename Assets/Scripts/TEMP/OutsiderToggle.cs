using System;
using UnityEngine;

public class OutsiderToggle : MonoBehaviour
{
    [SerializeField] private Magnet magnetReference;
    public bool toggle;

    private void Update()
    {
        magnetReference.OutsideMagnetToggle = toggle;
    }
}
