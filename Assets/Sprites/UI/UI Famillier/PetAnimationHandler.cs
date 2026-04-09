using UnityEngine;

public class PetAnimationHandler : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public void OnPetAnimationEnd()
    {
        if (inventoryManager != null)
        {
            inventoryManager.OnPetAnimationEnd();
        }
        else
        {
            Debug.LogWarning("InventoryManager non assigné !");
        }
    }
}