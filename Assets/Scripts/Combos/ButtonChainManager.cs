using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChainManager : MonoBehaviour
{
    [SerializeField]
    private patternButtonChain[] patterns;

    [SerializeField] private Image[] images;
    [SerializeField] private int comboResetDelay;

    private bool isInCombo = false;
    private bool isInPattern = true;
    
    private ButtonChainObject playerChain = new ButtonChainObject();

    private Coroutine currentCoroutine;
    
    
    
    public static event Action<string> OnPattern;
    
    private void OnNorthButton()
    {
        addInputToChain(ButtonChainObject.direction.north);
    }
    
    private void OnEastButton()
    {
        addInputToChain(ButtonChainObject.direction.east);    
    }
    
    private void OnSouthButton()
    {
        addInputToChain(ButtonChainObject.direction.south);
    }
    
    private void OnWestButton()
    {
        addInputToChain(ButtonChainObject.direction.west);
    }

    private void addInputToChain(ButtonChainObject.direction dir)
    {
        if (!isInCombo)
        {
            currentCoroutine = StartCoroutine(comboTime());
        }
        playerChain.Directions.Add(dir);
        verifyChain();
    }

    private void verifyChain()
    {
        if (playerChain.Directions.Count <= 3  && isInPattern)
        {
            if (playerChain.Directions[playerChain.Directions.Count - 1] ==
                patterns[0].Directions[playerChain.Directions.Count - 1])
            {
                activateIcons(playerChain.Directions.Count - 1);
            }
        }
        else
        {
            isInPattern = false;
        }
        
        
        for (int i = 0; i < patterns.Length; i++)
        {
            if (AreListsEqual(playerChain.Directions, patterns[i].Directions))
            {
                playerChain.Directions.Clear();
                resetIcons();
                isInCombo = false;
                StopCoroutine(currentCoroutine);
                OnPattern?.Invoke(patterns[i].name);
            }
        }
        
        
    }
    
    bool AreListsEqual(List<ButtonChainObject.direction> list1, ButtonChainObject.direction[] list2)
    {
        if (list1.Count != list2.Length)
            return false;

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
                return false;
        }

        return true;
    }

    IEnumerator comboTime()
    {
        isInCombo = true;
        yield return new WaitForSeconds(comboResetDelay);
        isInCombo = false;
        playerChain.Directions.Clear();
        resetIcons();
    }

    private void activateIcons(int i)
    {
        images[i].color = Color.yellow;
    }
 
    private void resetIcons()
    {
        foreach (var VARIABLE in images)
        {
            VARIABLE.color = Color.white;
        }

        isInPattern = true;
    }
}
