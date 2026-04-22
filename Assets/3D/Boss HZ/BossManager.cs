using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BossManager : MonoBehaviour, IDamagable
{
    public float currentHeal;
    public float maxHealPoint;
    public Slider HealBarre;
    public Image cible;

    public float vitesseAttaque;
    public int attaque;

    public int porteeAttaqueBase;
    public int porteeAttaqueSpe1;
    public Vector2 porteeAttaqueSpe2;

    public float cooldownEntreAttaque;
    private float cooldownTimer;
    private float cooldownTimerStalactite=10;
     private float cooldownEntreAttaqueStalactite;

    public float speed;

    public bool FightStarted;
    private bool isAttacking;
    private int StalactiteCount;
    public float cooldownAttaqueSpe3=0;

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
 if (cooldownAttaqueSpe3 > 0)
{
    cooldownAttaqueSpe3 -= Time.deltaTime;
}
if (cooldownEntreAttaqueStalactite > 0)
{
    cooldownEntreAttaqueStalactite -= Time.deltaTime;
}
        // COOLDOWN + MOVEMENT
       if (cooldownTimer > 0)
{
    cooldownTimer -= Time.deltaTime;

    // 👉 Priorité dash SI vraiment loin
    if (distance > porteeAttaqueSpe2.x * 1.5f)
    {
        if (!isAttacking)
            StartCoroutine(Dash());
    }
    else
    {
        // 👉 Toujours bouger sinon
        HandleMovement();
    }

    return;
}
if (cooldownEntreAttaqueStalactite <= 0)
        {
            ChuteStalactite();
        }

        // DASH PRIORITY
        if (distance > porteeAttaqueSpe2.x)
        {
            StartCoroutine(Dash());
            return;
        }
        int StalactiteCount = GameObject.FindGameObjectsWithTag("Stalactite").Length;

        // ATTACK LIST
        List<(System.Action action, float weight)> attaquesPonderees = new List<(System.Action, float)>();

float distNorm = Mathf.InverseLerp(porteeAttaqueSpe2.x, porteeAttaqueBase, distance);
// distNorm = 0 → loin
// distNorm = 1 → proche

// 👉 Attaque Base (50% → 75%)
if (distance <= porteeAttaqueBase)
{
    float weightBase = Mathf.Lerp(0.5f, 0.75f, distNorm);
    attaquesPonderees.Add((AttaqueBase, weightBase));
}

// 👉 Spe1 (poids fixe)
if (distance <= porteeAttaqueSpe1)
{
    attaquesPonderees.Add((AttaqueSpe1, 0.4f));
}

// 👉 Spe2 (poids fixe)
if (porteeAttaqueSpe2.y <= distance && distance <= porteeAttaqueSpe2.x)
{
    attaquesPonderees.Add((AttaqueSpe2, 0.5f));
}
// 👉 Spe3 dépend du nombre de stalactites
if (StalactiteCount > 0 && cooldownAttaqueSpe3 <= 0 )
{
    float weightSpe3 = Mathf.Clamp(StalactiteCount * 0.3f, 0.3f, 2f);
    attaquesPonderees.Add((AttaqueSpe3, weightSpe3));
}

// 👉 Spe3 dépend du nombre de stalactites
float attackChance = 0.8f; // 80% attaque, 20% move

if (attaquesPonderees.Count > 0 && Random.value < attackChance)
{
    float totalWeight = 0f;

    foreach (var atk in attaquesPonderees)
        totalWeight += atk.weight;

    float rand = Random.value * totalWeight;

    foreach (var atk in attaquesPonderees)
    {
        if (rand < atk.weight)
        {
            atk.action.Invoke();
            return;
        }

        rand -= atk.weight;
    }
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

    float influence = 0.5f + (StalactiteCount * 0.2f);

    if (closestStalactite != null && stalDist < playerDist * influence)
    {
        Debug.Log("[MOVE] Spe4");
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
    // Direction SANS Y
    Vector3 direction = player.transform.position - transform.position;
    direction.y = 0f;
    direction.Normalize();

    float distance = Vector3.Distance(transform.position, player.transform.position);

    // ✔ Stop si trop proche
    if (distance <= 5f)
    {
        // ✔ Bonus : cooldown accéléré
        cooldownTimer -= Time.deltaTime * 2f; // tweak si besoin
        return;
    }

    // Déplacement normal
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

  Vector3 targetPos = player.transform.position;
targetPos.y = transform.position.y;

Vector3 dir = (targetPos - transform.position).normalized;

Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0, -90, 0);

GameObject cone = Instantiate(conePrefab, transform.position, rot, transform);

    // 3. Distance à plat
    float distanceToPlayer = Vector3.Distance(transform.position, targetPos);

    float targetDistance = Mathf.Min(distanceToPlayer, porteeAttaqueBase * 0.5f);

    float movedDistance = 0f;

 yield return new WaitForSeconds(vitesseAttaque/2);
    // 4. Dash contrôlé
    while (movedDistance < targetDistance)
    {
        float step = speed * 3f * Time.deltaTime;

        if (movedDistance + step > targetDistance)
            step = targetDistance - movedDistance;

        transform.position += dir * step;
        movedDistance += step;

        yield return null;
    }

    // 5. Télégraphe
    yield return new WaitForSeconds(vitesseAttaque/2);

    // 6. Hit detection
    Collider[] hits = Physics.OverlapSphere(cone.transform.position, porteeAttaqueBase, playerLayer);

    foreach (Collider hit in hits)
    {
        if (hit.CompareTag("Player"))
        {
            Debug.Log("Player touché par AttaqueBase");
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
            Random.Range(-10f, 10f),
            0,
            Random.Range(-10f, 10f)
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
    GameObject mur = Instantiate(
        murGlacePrefab,
        transform.position + dir * 2f,
        Quaternion.LookRotation(dir)
    );

    yield return new WaitForSeconds(vitesseAttaque);

    // 2. Tir depuis 4 points du mur
    for (int i = 0; i < 4; i++)
    {
        Vector3 offset = new Vector3(i - 1.5f, 0, 0);
        Vector3 spawnPos = mur.transform.position + mur.transform.right * offset.x;

        Vector3 shootDir = (spawnPos - transform.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // 👉 ORIENTATION VISUELLE = direction du tir
        proj.transform.forward = shootDir;

        // 👉 PHYSIQUE
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
    float delay = 0.5f;

    yield return new WaitForSeconds(delay);

    float radius = 1.5f; // petit décalage autour du boss

    for (int i = 0; i < nbProjectiles; i++)
    {
        float angle = i * Mathf.PI * 2f / nbProjectiles;

        // 👉 position autour du boss
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

        Vector3 spawnPos = transform.position + offset;

        // 👉 direction vers l'extérieur
        Vector3 dir = offset.normalized;

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

         proj.transform.forward = dir;

        // 👉 PHYSIQUE
        proj.GetComponent<Rigidbody>().linearVelocity = dir * 0.0f;
    }

    cooldownAttaqueSpe3 = 30f;
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
if (!closest) yield break;
   while (true)
{
    if (closest == null)
    {
        isAttacking = false;
        yield break;
    }

    Vector3 targetPos = closest.transform.position;

    float distance = Vector3.Distance(transform.position, targetPos);

    if (distance <= 1f)
        break;

    Vector3 dir = (targetPos - transform.position).normalized;
    transform.position += dir * speed * Time.deltaTime;

    yield return null;

}

    Destroy(closest);

    Vector3 cible = player.transform.position;

    GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

    Vector3 dirShot = (cible - transform.position).normalized;
    proj.transform.forward = dirShot;
    proj.GetComponent<Rigidbody>().linearVelocity = dirShot * 12f;

    yield return new WaitForSeconds(0.5f);

    cooldownTimer = cooldownEntreAttaque;
    isAttacking = false;
}
    public void ChuteStalactite()
{
    StartCoroutine(ChuteStalactiteRoutine());
}

IEnumerator ChuteStalactiteRoutine()
{
    
    isAttacking = true;
    

    List<Vector3> positions = new List<Vector3>();
    List<GameObject> zones = new List<GameObject>();

        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-10f, 10f),
            0,
            Random.Range(-10f, 10f)
        );

        positions.Add(randomPos);
        zones.Add(Instantiate(zoneRougePrefab, randomPos, Quaternion.identity));
    

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

    cooldownTimerStalactite = cooldownEntreAttaqueStalactite;
    isAttacking = false;
}
}
