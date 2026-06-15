using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeplacementUnite2D : MonoBehaviour
{
    public float speed;
    public float dashDuration;
    public Rigidbody2D rb2d;
    Vector3 dir;
    public bool isDashing;
    public bool isdash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         dir = Gamepad.current.leftStick.ReadValue();
       /* if (Input.GetKeyDown(KeyCode.W)) dir = new Vector3 (0,1,0);
        if (Input.GetKeyDown(KeyCode.S)) dir = new Vector3 (0,-1,0);
        if (Input.GetKeyDown(KeyCode.A)) dir = new Vector3 (-1,0,0);
        if (Input.GetKeyDown(KeyCode.D)) dir = new Vector3 (1,0,0);*/
        Deplacement();

        if (Gamepad.current.rightTrigger.ReadValue()>0.2f && !isDashing) StartCoroutine(Dash());
        //if (Input.GetKeyDown(KeyCode.E) && Dash()!=null) StartCoroutine(Dash());
    }
    IEnumerator Dash()
    {
        isDashing=true;
        isdash=true;
        Vector3 dash = -dir*2;
        rb2d.AddForce(dash);
        yield return new WaitForSeconds(dashDuration);
        rb2d.linearVelocity = Vector2.zero;
        isdash=false;
        yield return new WaitForSeconds(0.5f);
        isDashing=false;
    }
    void Deplacement()
    {
        transform.position -= speed*dir*Time.deltaTime;
    }
}
