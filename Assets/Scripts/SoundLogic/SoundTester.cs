using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundTester : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        #region ButtonPress

        button.onClick.AddListener(() =>
        {
            SoundEvents.PlaySFX("SoundExampleName");
        });

        #endregion
    }
}
