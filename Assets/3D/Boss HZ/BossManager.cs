using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public float currentHeal;
    public float maxHealPoint;
    public Slider HealBarre;
    public Image cible;

    public float vitesseAttaque;
    public int attaque;

    public int porteeAttaqueBase;
    public int porteeAttaqueSpe1;
    public int porteeAttaqueSpe2;

    public float cooldownEntreAttaque;
    private float cooldownTimer;

    public float speed;

    public bool FightStarted;
    private bool isAttacking;
    private int StalactiteCount;

    public SphereCollider zoneChasse;
    public GameObject player;
    public GameObject conePrefab;
public GameObject zoneRougePrefab;
public GameObject stalactitePrefab;
public GameObject projectilePrefab;
public GameObject murGlacePrefab;

public LayerMask playerLayer;
    private HashSet<int> receivedAttacks = new HashSet<int>();


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHeal = maxHealPoint;
    }

   void Update()
    {
        if (!FightStarted || player == null) return;
        if (isAttacking) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        // COOLDOWN + MOVEMENT
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;

            if (distance > porteeAttaqueSpe2)
                StartCoroutine(Dash());
            else
                HandleMovement();

            return;
        }

        // DASH PRIORITY
        if (distance > porteeAttaqueSpe2)
        {
            StartCoroutine(Dash());
            return;
        }

        // ATTACK LIST
        List<System.Action> attaquesPossibles = new List<System.Action>();

        StalactiteCount = GameObject.FindGameObjectsWithTag("Stalactite").Length;

        if (distance <= porteeAttaqueBase)
            attaquesPossibles.Add(AttaqueBase);

        if (distance <= porteeAttaqueSpe1)
            attaquesPossibles.Add(AttaqueSpe1);

        if (distance <= porteeAttaqueSpe2)
            attaquesPossibles.Add(AttaqueSpe2);

        if ( StalactiteCount > 0)
            attaquesPossibles.Add(AttaqueSpe3);

        Debug.Log($"[AI] Attaques possibles: {attaquesPossibles.Count}");

        if (attaquesPossibles.Count > 0)
        {
            int rand = Random.Range(0, attaquesPossibles.Count);
            attaquesPossibles[rand].Invoke();
        }
        else
        {
            HandleMovement();
        }
    }
      private void UpdateHealthBar()
{
    HealBarre.value = (float)currentHeal / (float)maxHealPoint;
}

public void TakeDamage(float damage, int attackID)
{
    if (receivedAttacks.Contains(attackID)) return;

    receivedAttacks.Add(attackID);
    StartCoroutine(ClearAttackID(attackID));

    currentHeal -= damage;

    if (currentHeal <= 0)
    {
        Die();
    }
    else
    {
        UpdateHealthBar();
    }

    Debug.Log($"Enemy took {damage} damage");
}

IEnumerator ClearAttackID(int id)
{
    yield return new WaitForSeconds(0.5f);
    receivedAttacks.Remove(id);
}

private void Die()
{
    Destroy(gameObject);
}
    // ---------------- MOVEMENT LOGIC ----------------

    void HandleMovement()
    {
        GameObject closestStalactite = GetClosestStalactite();

        float playerDist = Vector3.Distance(transform.position, player.transform.position);

        float stalDist = closestStalactite != null
            ? Vector3.Distance(transform.position, closestStalactite.transform.position)
            : Mathf.Infinity;


        // règle : joueur prioritaire si plus proche que danger environnemental
        if (closestStalactite != null && playerDist*(StalactiteCount*0.2)-stalDist > 0)
        {
            Debug.Log("[MOVE] Spe3 (interaction environnement)");
            AttaqueSpe4(closestStalactite);
        }
        else
        {
            DeplacementVersJoueur();
        }
    }

    GameObject GetClosestStalactite()
    {
        GameObject[] stals = GameObject.FindGameObjectsWithTag("Stalactite");

        if (stals.Length == 0) return null;

        GameObject closest = stals[0];
        float minDist = Vector3.Distance(transform.position, closest.transform.position);

        foreach (GameObject s in stals)
        {
            float d = Vector3.Distance(transform.position, s.transform.position);

            if (d < minDist)
            {
                minDist = d;
                closest = s;
            }
        }

        return closest;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FightStarted = true;
            cible.enabled=true;
            Debug.Log("Combat commencé !");
        }
    }

    void DeplacementVersJoueur()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }


    IEnumerator Dash()
    {
        isAttacking = true;

        Vector3 direction = (player.transform.position - transform.position).normalized;

        float dashTime = 0.5f;
        float timer = 0;

        while (timer < dashTime)
        {
            transform.position += direction * speed * 3 * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        cooldownTimer = cooldownEntreAttaque;
        isAttacking = false;
    }

public void AttaqueBase()
{
    StartCoroutine(AttaqueBaseRoutine());
}

IEnumerator AttaqueBaseRoutine()
{
    isAttacking = true;

    // 1. Spawn cone devant le boss
    GameObject cone = Instantiate(conePrefab, transform.position, transform.rotation);

    // 2. Attente (prévisualisation)
    yield return new WaitForSeconds(vitesseAttaque);

    // 3. Check si player dans le cone
    Collider[] hits = Physics.OverlapSphere(cone.transform.position, porteeAttaqueBase, playerLayer);

    foreach (Collider hit in hits)
    {
        if (hit.CompareTag("Player"))
        {
            Debug.Log("Player touché par AttaqueBase");
            // TODO : appliquer dégâts
        }
    }

    Destroy(cone);

    cooldownTimer = cooldownEntreAttaque;
    isAttacking = false;
}
    public void AttaqueSpe1()
{
    StartCoroutine(AttaqueSpe1Routine());
}

IEnumerator AttaqueSpe1Routine()
{
    isAttacking = true;

    List<Vector3> positions = new List<Vector3>();
    List<GameObject> zones = new List<GameObject>();

    // 1. Générer 4 zones aléatoires
    for (int i = 0; i < 4; i++)
    {
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-5f, 5f),
            0,
            Random.Range(-5f, 5f)
        );

        positions.Add(randomPos);
        zones.Add(Instantiate(zoneRougePrefab, randomPos, Quaternion.identity));
    }

    yield return new WaitForSeconds(vitesseAttaque);

    // 2. Dégâts + spawn stalactites
    foreach (Vector3 pos in positions)
    {
        Collider[] hits = Physics.OverlapSphere(pos, 1.5f, playerLayer);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Player touché Spe1");
                // TODO dégâts
            }
        }

        Instantiate(stalactitePrefab, pos, Quaternion.identity);
    }

    // Clean zones
    foreach (GameObject z in zones)
        Destroy(z);

    cooldownTimer = cooldownEntreAttaque;
    isAttacking = false;
}
public void AttaqueSpe2()
{
    StartCoroutine(AttaqueSpe2Routine());
}

IEnumerator AttaqueSpe2Routine()
{
    isAttacking = true;

    Vector3 dir = (player.transform.position - transform.position).normalized;

    // 1. Spawn mur
    GameObject mur = Instantiate(murGlacePrefab, transform.position + dir * 2f, Quaternion.LookRotation(dir));

    yield return new WaitForSeconds(vitesseAttaque);

    // 2. Tir depuis 4 points du mur
    for (int i = 0; i < 4; i++)
    {
        Vector3 offset = new Vector3(i - 1.5f, 0, 0);
        Vector3 spawnPos = mur.transform.position + mur.transform.right * offset.x;

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector3 shootDir = (spawnPos - transform.position ).normalized;

        proj.GetComponent<Rigidbody>().linearVelocity = shootDir * 10f;
    }

    Destroy(mur);

    cooldownTimer = cooldownEntreAttaque;
    isAttacking = false;
}
public void AttaqueSpe3()
{
    StartCoroutine(AttaqueSpe3Routine());
}

IEnumerator AttaqueSpe3Routine()
{
    isAttacking = true;

    int nbProjectiles = 24;

    for (int i = 0; i < nbProjectiles; i++)
    {
        float angle = i * Mathf.PI * 2 / nbProjectiles;

        Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        proj.GetComponent<Rigidbody>().linearVelocity = dir * 8f;
    }

    yield return new WaitForSeconds(0.2f);

    cooldownTimer = cooldownEntreAttaque;
    isAttacking = false;
}
public void AttaqueSpe4(GameObject target)
{
    StartCoroutine(AttaqueSpe4Routine(target));
}

IEnumerator AttaqueSpe4Routine(GameObject closest)
{
    isAttacking = true;

    if (closest == null)
    {
        isAttacking = false;
        yield break;
    }

   while (Vector3.Distance(transform.position, closest.transform.position) > 1f)
{
    if (closest == null)
    {
        isAttacking = false;
        yield break;
    }

    Vector3 dir = (closest.transform.position - transform.position).normalized;
    transform.position += dir * speed * Time.deltaTime;

    yield return null;
}

    Destroy(closest);

    Vector3 cible = player.transform.position;

    GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

    Vector3 dirShot = (cible - transform.position).normalized;

    proj.GetComponent<Rigidbody>().linearVelocity = dirShot * 12f;

    yield return new WaitForSeconds(0.5f);

    cooldownTimer = cooldownEntreAttaque;
    isAttacking = false;
}
}
