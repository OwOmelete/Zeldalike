using System;
using UnityEngine;

public class SimonDisplay : MonoBehaviour
{
    [SerializeField] private float displayedTime;
    [SerializeField] private float delayBetweenColors;
    [SerializeField] private float delayBetweenSequences;

    [SerializeField] private GameObject blueObject;
    [SerializeField] private GameObject redObject;

    [SerializeField] private SimonManager _simonManager;
    
    public enum color
    {
        red,
        blue
    }

    public enum State
    {
        colorDisplay,
        waitingColor,
        waitingSequence
    }

    private float timer = 0;
    private State currentState = State.colorDisplay;

    
    
    private int currentIndex;
    

    private void FixedUpdate()
    {
        if (_simonManager.hasWon) return;
        if (_simonManager.isTrying)
        {
            if (currentIndex != 0)
            {
                ResetSequence();
            }
            return;
        }
        
        
        switch (currentState)
        {
            case State.colorDisplay:

                if (Time.time - timer >= displayedTime)
                {
                    if (currentIndex == _simonManager.sequence.Length - 1)
                    {
                        switchState(State.waitingSequence);
                    }
                    else
                    {
                        switchState(State.waitingColor);
                    }
                }
                
                break;
            case State.waitingColor:

                if (Time.time - timer >= delayBetweenColors)
                {
                    switchState(State.colorDisplay);
                }

                break;
            case State.waitingSequence:

                if (Time.time - timer >= delayBetweenSequences)
                {
                    switchState(State.colorDisplay);
                }
                
                break;
        }
    }


    private void switchState(State state)
    {

        timer = Time.time;
        
        switch (state)
        {
            case State.colorDisplay:
                                
                currentState = State.colorDisplay;
                colorDisplay();
                
                break;
            case State.waitingColor:

                currentIndex += 1;
                currentState = State.waitingColor;
                resetColors();

                break;
            case State.waitingSequence:

                currentIndex = 0;
                currentState = State.waitingSequence;
                resetColors();
                
                break;
        }
        
        
    }

    private void colorDisplay()
    {
        if (_simonManager.sequence[currentIndex] == color.blue)
        {
            blueObject.SetActive(true);
        }
        else
        {
            redObject.SetActive(true);
            
        }
    }

    private void resetColors()
    {
        redObject.SetActive(false);
        blueObject.SetActive(false);
    }

    public void ResetSequence()
    {
        currentIndex = 0;
        timer = Time.time;
        switchState(State.waitingSequence);
    }


}
