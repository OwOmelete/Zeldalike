using System;
using UnityEngine;

public class CharacterDirection : MonoBehaviour
{
    private float currentRotation;
    [SerializeField] private Sprite[] spriteList;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform cam;
    [SerializeField] private SpriteRenderer sr;
    
    

    private void Update()
    {
        currentRotation = (player.transform.rotation * Quaternion.Inverse(cam.rotation) ).eulerAngles.y;
        sr.sprite = spriteList[angleToInt(currentRotation)];
    }

    private int angleToInt(float angle)
    {
        angle -= 45/2;
        if (angle < 0) angle += 360;

        return Mathf.FloorToInt(angle / 45);

    }

    
}
