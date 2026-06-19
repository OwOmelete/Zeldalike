using System;
using JetBrains.Annotations;
using UnityEngine;

public class NoyauManager : MonoBehaviour
{

     public int currentRemplissage;
     public static NoyauManager INSTANCE;


     private void Awake()
     {
          if (INSTANCE != null)
          {
               Destroy(this);
          }
          else
          {
               INSTANCE = this;
          }
     }

     public void increaseRemplissage()
     {
          
          currentRemplissage++;
          CinematiqueManager.INSTANCE.playAnim(currentRemplissage-1);
     }
     
}
