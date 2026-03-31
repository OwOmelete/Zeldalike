using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChainManager : MonoBehaviour
{
    [SerializeField]
    private patternButtonChain[] patterns;

    [SerializeField] private int comboResetDelay;

    private bool isInCombo = false;
    
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
        foreach (var VARIABLE in playerChain.Directions)
        {
            Debug.Log(VARIABLE);
        }
        
        
        for (int i = 0; i < patterns.Length; i++)
        {
            if (AreListsEqual(playerChain.Directions, patterns[i].Directions))
            {
                playerChain.Directions.Clear();
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
    }
    
}
