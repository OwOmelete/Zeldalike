using System;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public Transform target;
    public Image sprite;

    private void Update()
    {
        if (target) transform.position = target.transform.position;
        else sprite.enabled = false;

    }
}
