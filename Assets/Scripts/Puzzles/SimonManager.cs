using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonManager : MonoBehaviour, IDamagable
{

    [SerializeField] private SimonDisplay _simonDisplay;

    [SerializeField] private float afkDelay;

    [SerializeField] private float throwForce;
    [SerializeField] private float releaseDelay;

    [SerializeField] private Transform objectRef;
    [SerializeField] private Transform ThrowingDirection;

    [SerializeField] private List<Door> _door;
    
    private HashSet<int> receivedAttacks = new HashSet<int>();

    private List<Rigidbody> rbList = new List<Rigidbody>(16);    

    
    

    private int currentIndex;
    
    public SimonDisplay.color[] sequence;

    public bool isTrying;
    public bool hasWon;

    private float timer;
    
    
    
    
    public void TakeDamage(float damage, int attackID, InvoDataInstance data)
    {
        if (receivedAttacks.Contains(attackID))
            return;

        receivedAttacks.Add(attackID);

        if (data.currentTemperature == InvoDataInstance.temperature.hot)
        {
            hot();
        }

        if (data.currentTemperature == InvoDataInstance.temperature.cold)
        {
            cold();
        }
        
        rbList.Add(data.rb);
        StartCoroutine(deactivate(data.rb));
        data.rb.linearVelocity = Vector3.zero;
        data.rb.transform.position = objectRef.transform.position;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //hot();
            foreach (var door in _door)
            {
                door.OpenDoor();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var door in _door)
            {
                door.Close();
            }
        }

        if (Time.time - timer >= afkDelay && isTrying)
        {
            Dommage();
        }
        
    }


    private void hot()
    {
        timer = Time.time;
        
        
        if (sequence[currentIndex] == SimonDisplay.color.red)
        {
            currentIndex += 1;
            isTrying = true;
            if (currentIndex >= sequence.Length)
            {
                Bravo();
            }
        }
        else
        {
            currentIndex = 0;
            isTrying = false;
            Dommage();
        }
    }

    private void cold()
    {
        timer = Time.time;
        
        if (sequence[currentIndex] == SimonDisplay.color.blue)
        {
            currentIndex += 1;
            isTrying = true;
            if (currentIndex >= sequence.Length)
            {
                Bravo();
            }
        }
        else
        {
            currentIndex = 0;
            isTrying = false;
            Dommage();
        }
    }
    
    IEnumerator deactivate(Rigidbody rb)
    {
        yield return new WaitForEndOfFrame();
        if (isTrying)
        {
            rb.isKinematic = true;
        }
    }

    private void Bravo()
    {
        hasWon = true;
        ReleaseInvos();
        if (_door != null)
        {
            foreach (var door in _door)
            {
                door.OpenDoor();
            }
        }
        Debug.Log("bravo");
    }

    private void Dommage()
    {
        isTrying = false;
        _simonDisplay.ResetSequence();
        currentIndex = 0;
        ReleaseInvos();
    }


    private void ReleaseInvos()
    {
        /*foreach (var rb in rbList)
        {
            rb.isKinematic = false;   
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(ThrowingDirection.position - objectRef.position, ForceMode.Impulse);
        }
        rbList.Clear();*/

        StartCoroutine(Release());
    }

    IEnumerator Release()
    {
        while (rbList.Count >= 1)
        {
            Rigidbody rb = rbList[^1];
            rb.isKinematic = false;   
            rb.linearVelocity = Vector3.zero;
            rb.AddForce((ThrowingDirection.position - objectRef.position) * throwForce, ForceMode.Impulse);
            rbList.RemoveAt(rbList.Count-1);
            yield return new WaitForSeconds(releaseDelay);
        }
    }
    
}
