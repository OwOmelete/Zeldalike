using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUiManager : MonoBehaviour
{
    [SerializeField] private int currentNumOfFamiliars;
    [SerializeField] private int maxNumOfFamiliars;
    
    [SerializeField] private TMP_Text familiarText;
    
    [SerializeField] private int currentHeat;
    [SerializeField] private int maxHeat;
    
    private Slider heatSlider;

    private void Update()
    {
        familiarText.text = $"{currentNumOfFamiliars}/{maxNumOfFamiliars}";
    }
}
