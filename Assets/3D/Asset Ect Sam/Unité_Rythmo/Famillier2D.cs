using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Famillier2D : MonoBehaviour
{
    public Animator animator;
    public bool IsBusy;
    public GameObject player;
    public float speed;

    public void StartAttack(GameObject target, Combat2D playerPosition, float speed)
    {
        StartCoroutine(AttackRoutine(target, playerPosition.transform.position, speed));
        player = playerPosition.gameObject;
    }
    public void refToPlayer(Combat2D playerPosition)
    {
        player = playerPosition.gameObject;
    }
    void Update()
    {
        Vector3 playerDistance = -transform.position + player.transform.position;
        Vector3 dir = playerDistance.normalized;
        transform.position += dir*Time.deltaTime*speed;
        if (playerDistance.magnitude>500f) transform.position = player.transform.position;
        
    }

    private IEnumerator AttackRoutine(GameObject target, Vector3 playerPosition, float speed)
    {
        IsBusy = true;

        if (target == null)
        {
            IsBusy = false;
            yield break;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            IsBusy = false;
            yield break;
        }

        transform.position = playerPosition;

        yield return new WaitForSeconds(0.05f);

        if (Gamepad.current != null)
        {
            rb.linearVelocity = Gamepad.current.leftStick.ReadValue() * 400;
        }

        while (target != null)
        {
            Vector2 dir = (target.transform.position - transform.position);

            if (dir.magnitude < 20f)
                break;

            rb.linearVelocity = dir.normalized * speed;

            yield return null;
        }
        
        if (animator != null)
            animator.SetTrigger("Attack");

        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(2f);

        IsBusy = false;
        animator.SetTrigger("NotAttack");
        transform.position = player.transform.position;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dummy")&&true)
        {
            collision.GetComponent<Enemy2D>().TakeDamage();
            //IsBusy = false;
        }
    }
}