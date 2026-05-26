using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundTest : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        #region ButtonPress

        button.onClick.AddListener(() =>
        {
            SoundEvents.PlayJump();
        });

        #endregion
    }
}
