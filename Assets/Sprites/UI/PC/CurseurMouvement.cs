
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CurseurMouvement : MonoBehaviour
{
    public Image curseur;
    public float speed;
    public GameObject TowerDefence;
    public GameObject Cliker;
    public GameObject PC;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void OnEnable()
    {
        transform.position = new Vector3 (500,400,0 );
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Gamepad.current.leftStick.ReadValue();
        transform.position += dir*Time.unscaledDeltaTime*speed;
        if(Gamepad.current.buttonSouth.wasPressedThisFrame)OnInput();
    }
    void OnInput()
{
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.0f);
    
    foreach (var hitCollider in hitColliders)
    {
       if(hitCollider.CompareTag("Obstacle"))
            {
                PC.SetActive(false);
                TowerDefence.SetActive(true);
            }
            
       if(hitCollider.CompareTag("Pointer"))
            {
                PC.SetActive(false);
                 Cliker.SetActive(true);
            }
       
    }
}
}
