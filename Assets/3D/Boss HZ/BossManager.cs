using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class BossManager : MonoBehaviour, IDamagable
{
    [Header("Santé")]
    public EnnemyHeatSystem HeatSystem;
    public Image cible;

    [Header("Attaque")]
    public float vitesseAttaque;
    public int attaque;

    public int porteeAttaqueBase;
    public int porteeAttaqueSpe1;
    public Vector2 porteeAttaqueSpe2;

    [Header("Cooldowns")]
    public float cooldownEntreAttaque;
    private float cooldownTimer;
    private float cooldownEntreAttaqueStalactite = 10f;
    private float cooldownTimerStalactite;
    public float cooldownAttaqueSpe3 = 0f;
    public float cooldownEntreDashPilier = 8f;
    private float cooldownDashPilier = 0f;

    [Header("Déplacement")]
    public float speed;

    [Header("État")]
    public bool FightStarted;
    private bool isAttacking;

    [Header("Références")]
    public SphereCollider zoneChasse;
    private GameObject player;
    public GameObject lookAtPlayer;
    public GameObject conePrefab;
    public GameObject zoneRougePrefab;
    public List<GameObject> stalactitePrefab = new List<GameObject>();
    public GameObject murGlacePrefab;
    public List<GameObject> projectilePrefab = new List<GameObject>();
    public List<Vector3> positions = new List<Vector3>();
    public List<GameObject> zones = new List<GameObject>();
    public Animator animator;
    public GameObject MainCamera;
    [SerializeField] PlayerHealth playerHealth;
    public CombatZone _combatZone;


    [Header("Compteurs")]
    public int StalactiteCount;
    public int zoneCount;
    public int projectileCount;

    public LayerMask playerLayer;
    private HashSet<int> receivedAttacks = new HashSet<int>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
     private void MiseAngle()
{
    Vector3 direction = player.transform.position - transform.position;
    direction.y = 0f;

    if (direction.sqrMagnitude < 0.001f)
        return;

    float angle = Vector3.SignedAngle(
        MainCamera.transform.forward,
        direction.normalized,
        Vector3.up
    );

    if (angle < 0)
        angle += 360f;

    int dir = AngleToInt(angle);

    Vector2[] directions =
    {
        new Vector2( 0, -1),
        new Vector2(-1, -1),
        new Vector2(-1,  0), 
        new Vector2(-1,  1), 
        new Vector2( 0,  1), 
        new Vector2( 1,  1), 
        new Vector2( 1,  0), 
        new Vector2( 1, -1)
    };

    animator.SetFloat("x", directions[dir].x);
    animator.SetFloat("y", directions[dir].y);
}

private int AngleToInt(float angle)
{
    const int nbDirections = 8;
    float sectorSize = 360f / nbDirections;

    angle += sectorSize * 0.5f;
    angle %= 360f;

    return Mathf.FloorToInt(angle / sectorSize);
}


    void Update()
    {
        if(!HeatSystem.isAlive) Die();
        if (!FightStarted || player == null) return;

        // Décrémentation de tous les cooldowns — toujours, même pendant une attaque
        if (cooldownTimer > 0)          cooldownTimer          = Mathf.Max(0f, cooldownTimer          - Time.deltaTime);
        if (cooldownAttaqueSpe3 > 0)    cooldownAttaqueSpe3    = Mathf.Max(0f, cooldownAttaqueSpe3    - Time.deltaTime);
        if (cooldownTimerStalactite > 0) cooldownTimerStalactite = Mathf.Max(0f, cooldownTimerStalactite - Time.deltaTime);
        if (cooldownDashPilier > 0)     cooldownDashPilier     = Mathf.Max(0f, cooldownDashPilier     - Time.deltaTime);

        if (isAttacking) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
       Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;

        
        lookAtPlayer.transform.rotation = Quaternion.LookRotation(direction);
        MiseAngle();
        
        if (cooldownTimer > 0)
        {
            if (distance > porteeAttaqueSpe2.x * 1.5f)
                StartCoroutine(Dash());
            else
                HandleMovement();
            return;
        }

        // Stalactite passive désactivé pour l'instant
        // if (cooldownTimerStalactite <= 0)
        //     ChuteStalactite();

        if (distance > porteeAttaqueSpe2.x)
        {
            StartCoroutine(Dash());
            return;
        }

        // Construction des attaques pondérées
        List<(System.Action action, float weight)> attaquesPonderees = new List<(System.Action, float)>();

        float distNorm = Mathf.InverseLerp(porteeAttaqueSpe2.x, porteeAttaqueBase, distance);

        if (distance <= porteeAttaqueBase)
        {
            float weightBase = Mathf.Lerp(0.5f, 0.75f, distNorm);
            attaquesPonderees.Add((AttaqueBase, weightBase));
        }
        if (distance <= porteeAttaqueSpe1)
            attaquesPonderees.Add((AttaqueSpe1, 0.3f));

        if (porteeAttaqueSpe2.y <= distance && distance <= porteeAttaqueSpe2.x)
            attaquesPonderees.Add((AttaqueSpe2, 0.2f));

        if (StalactiteCount > 0 && cooldownAttaqueSpe3 <= 0)
        {
            float weightSpe3 = Mathf.Clamp(StalactiteCount * 0.3f, 0.3f, 2f);
            attaquesPonderees.Add((AttaqueSpe3, weightSpe3));
        }

        // Dash vers un pilier — plus probable s'il y a beaucoup de piliers
        if (StalactiteCount > 0 && cooldownDashPilier <= 0)
        {
            float weightDashPilier = Mathf.Clamp(StalactiteCount * 0.4f, 0.2f, 2f);
            attaquesPonderees.Add((AttaqueSpe5, weightDashPilier));
        }

        if (attaquesPonderees.Count > 0 && Random.value < 0.8f)
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


    public void TakeDamage(float damage, int attackID, InvoDataInstance data)
    {
        if (receivedAttacks.Contains(attackID)) return;
        receivedAttacks.Add(attackID);
        animator.SetTrigger("Hit");

        switch (data.currentTemperature)
        {
            case InvoDataInstance.temperature.cold:
                HeatSystem.reduceHeat(data.coldValue);
                break;
            case InvoDataInstance.temperature.hot:
                HeatSystem.increaseHeat(data.hotValue);
                break;
        }
    }

    IEnumerator ClearAttackID(int id)
    {
        yield return new WaitForSeconds(0.5f);
        receivedAttacks.Remove(id);
    }

    private void Die()
    {
        Destroy(gameObject);
        if (!HeatSystem.isAlive)
        {
            if (_combatZone!= null)
            {
                foreach (var door in _combatZone.doors)
                {
                    door.OpenDoor();
                } 
                if (_combatZone.bridgeCollider) _combatZone.bridgeCollider.SetActive(false);
            }
            Destroy(gameObject,1f);
        }
    }

    void HandleMovement()
    {
        animator.SetBool("isMoving",true);
        if (StalactiteCount > 0 && stalactitePrefab[StalactiteCount - 1].activeSelf)
        {
            GameObject closestStalactite = GetClosestStalactite();
            float playerDist = Vector3.Distance(transform.position, player.transform.position);

            float stalDist = closestStalactite != null
                ? Vector3.Distance(transform.position, closestStalactite.transform.position)
                : Mathf.Infinity;

            float influence = 0.5f + (StalactiteCount * 0.2f);

            if (closestStalactite != null && stalDist < playerDist * influence)
            {
                if (cooldownDashPilier <= 0 && Random.value < 0.4f)
                {
                    //Debug.Log("[MOVE] DashVersPilier");
                    AttaqueSpe5();
                }
                else
                {
                    //Debug.Log("[MOVE] AttaqueSpe4");
                    AttaqueSpe4(closestStalactite);
                }
                return;
            }
        }

        DeplacementVersJoueur();
    }

    GameObject GetClosestStalactite()
    {
        if (stalactitePrefab.Count == 0) return null;

        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject s in stalactitePrefab)
        {
            if (!s.activeSelf) continue;

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
            cible.enabled = true;
            Debug.Log("Combat commencé !");
            player = other.gameObject;
            playerHealth = other.GetComponent<PlayerHealth>();
            GetComponent<SphereCollider>().enabled=false;
        }
    }

    void DeplacementVersJoueur()
    {

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= 5f) return;

        transform.position += direction * speed * Time.deltaTime;
    }

    IEnumerator Dash()
    {
        isAttacking = true;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        float dashTime = 0.5f;
        float timer = 0f;

        while (timer < dashTime)
        {
            transform.position += direction * speed * 3f * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        cooldownTimer = cooldownEntreAttaque;
        isAttacking = false;
    }

    // ─────────────────────────────────────────────
    // ATTAQUE DE BASE
    // ─────────────────────────────────────────────
    public void AttaqueBase() => StartCoroutine(AttaqueBaseRoutine());

    IEnumerator AttaqueBaseRoutine()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        Vector3 targetPos = player.transform.position;
        targetPos.y = transform.position.y;
        Vector3 dir = (targetPos - transform.position).normalized;

        Quaternion rot = Quaternion.LookRotation(dir)*Quaternion.Euler(0,-90,0);
        conePrefab.SetActive(true);
        conePrefab.transform.localRotation = rot;
        conePrefab.transform.position = transform.position + dir*3;

        float distanceToPlayer = Vector3.Distance(transform.position, targetPos);
        float targetDistance = Mathf.Min(distanceToPlayer, porteeAttaqueBase * 0.5f);
        float movedDistance = 0f;

        yield return new WaitForSeconds(vitesseAttaque / 2f);

        while (movedDistance < targetDistance)
        {
            float step = Mathf.Min(speed * 3f * Time.deltaTime, targetDistance - movedDistance);
            transform.position += dir * step;
            movedDistance += step;
            yield return null;
        }

        yield return new WaitForSeconds(vitesseAttaque / 2f);

        Collider[] hits = Physics.OverlapSphere(conePrefab.transform.position, porteeAttaqueBase, playerLayer);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                 Debug.Log("Player touché par AttaqueBase");
                 DealDammage();
            }
               
        }

        conePrefab.SetActive(false);
        cooldownTimer = cooldownEntreAttaque;
        isAttacking = false;
    }

    // ─────────────────────────────────────────────
    // ATTAQUE SPÉ 1 — Zones + Stalactites
    // ─────────────────────────────────────────────
    public void AttaqueSpe1() => StartCoroutine(AttaqueSpe1Routine());

    IEnumerator AttaqueSpe1Routine()
    {
        isAttacking = true;
        animator.SetBool("isMoving",false);

        List<int> zonesActivees = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            if (zoneCount >= zones.Count) zoneCount = 0;

            if (!zones[zoneCount].activeSelf)
            {
                Vector3 randomPos = transform.position + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                positions[zoneCount] = randomPos;
                zones[zoneCount].SetActive(true);
                zones[zoneCount].transform.position = randomPos;
                zonesActivees.Add(zoneCount);
                zoneCount++;
            }
        }

        yield return new WaitForSeconds(vitesseAttaque);

        foreach (int idx in zonesActivees)
        {
            if (StalactiteCount >= stalactitePrefab.Count) StalactiteCount = 0;

            if (!stalactitePrefab[StalactiteCount].activeSelf)
            {
                stalactitePrefab[StalactiteCount].SetActive(true);
                stalactitePrefab[StalactiteCount].transform.position = positions[idx];

                Collider[] hits = Physics.OverlapSphere(positions[idx], 1.5f, playerLayer);
                foreach (Collider hit in hits)
                {
                    if (hit.CompareTag("Player"))
                    {
                        Debug.Log("Player touché Spe1");
                        DealDammage();
                        
                    }
                        
                }

                StalactiteCount++;
            }
        }
        animator.SetBool("isMoving",true);
        foreach (GameObject z in zones) z.SetActive(false);

        cooldownTimer = cooldownEntreAttaque;
        isAttacking = false;
    }

    // ─────────────────────────────────────────────
    // ATTAQUE SPÉ 2 — Mur de glace + Projectiles
    // ─────────────────────────────────────────────
    public void AttaqueSpe2() => StartCoroutine(AttaqueSpe2Routine());

    IEnumerator AttaqueSpe2Routine()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        Vector3 dir = (player.transform.position - transform.position).normalized;

        murGlacePrefab.SetActive(true);
        murGlacePrefab.transform.position = transform.position + dir;
        murGlacePrefab.transform.LookAt(transform.position + dir * 2f);

        yield return new WaitForSeconds(vitesseAttaque);

        for (int i = 0; i < 4; i++)
        {
            if (projectileCount >= projectilePrefab.Count) projectileCount = 0;

            Vector3 offset = new Vector3(i - 1.5f, 0, 0);
            Vector3 spawnPos = murGlacePrefab.transform.position + murGlacePrefab.transform.right * offset.x;
            Vector3 shootDir = (spawnPos - transform.position).normalized;

            if (!projectilePrefab[projectileCount].activeSelf)
            {
                GameObject proj = projectilePrefab[projectileCount];
                proj.SetActive(true);
                proj.transform.position = spawnPos;
                proj.transform.rotation = Quaternion.identity;
                proj.transform.forward = shootDir;
                proj.GetComponent<Rigidbody>().linearVelocity = shootDir * 10f;
                projectileCount++;
            }
        }

        murGlacePrefab.SetActive(false);
        cooldownTimer = cooldownEntreAttaque;
        isAttacking = false;
    }

    // ─────────────────────────────────────────────
    // ATTAQUE SPÉ 3 — Tir radial
    // ─────────────────────────────────────────────
    public void AttaqueSpe3() => StartCoroutine(AttaqueSpe3Routine());

    IEnumerator AttaqueSpe3Routine()
    {
        isAttacking = true;
        animator.SetBool("isMoving",false);
        yield return new WaitForSeconds(0.5f);

        int nbProjectiles = 24;
        float radius = 1.5f;

        for (int i = 0; i < nbProjectiles; i++)
        {
            if (projectileCount >= projectilePrefab.Count) projectileCount = 0;

            float angle = i * Mathf.PI * 2f / nbProjectiles;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 spawnPos = transform.position + offset;
            Vector3 dir = offset.normalized;

            if (!projectilePrefab[projectileCount].activeSelf)
            {
                GameObject proj = projectilePrefab[projectileCount];
                proj.SetActive(true);
                proj.transform.position = spawnPos;
                proj.transform.rotation = Quaternion.identity;
                proj.transform.forward = dir;
                proj.GetComponent<Rigidbody>().linearVelocity = dir * 10f;
                projectileCount++;
            }
        }
        animator.SetBool("isMoving",true);
        cooldownAttaqueSpe3 = 30f;
        cooldownTimer = cooldownEntreAttaque;
        isAttacking = false;
    }

    // ─────────────────────────────────────────────
    // ATTAQUE SPÉ 4 — Marcher vers une stalactite
    // ─────────────────────────────────────────────
    public void AttaqueSpe4(GameObject target) => StartCoroutine(AttaqueSpe4Routine(target));

    IEnumerator AttaqueSpe4Routine(GameObject closest)
    {
        isAttacking = true;

        if (closest == null)
        {
            isAttacking = false;
            yield break;
        }

        while (closest != null && closest.activeSelf)
        {
            float distance = Vector3.Distance(transform.position, closest.transform.position);
            if (distance <= 1f) break;

            Vector3 dir = (closest.transform.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            yield return null;
        }

        if (closest != null)
            closest.SetActive(false);

        if (player != null)
        {
            animator.SetTrigger("Throw");
            Vector3 dirShot = (player.transform.position - transform.position).normalized;

            if (projectileCount >= projectilePrefab.Count) projectileCount = 0;

            if (!projectilePrefab[projectileCount].activeSelf)
            {
                GameObject proj = projectilePrefab[projectileCount];
                proj.SetActive(true);
                proj.transform.position = transform.position;
                proj.transform.rotation = Quaternion.identity;
                proj.transform.forward = dirShot;
                proj.GetComponent<Rigidbody>().linearVelocity = dirShot * 10f;
                projectileCount++;
            }
        }

        yield return new WaitForSeconds(0.5f);
        cooldownTimer = cooldownEntreAttaque;
        isAttacking = false;
    }

    // ─────────────────────────────────────────────
    // ATTAQUE SPÉ 5 — Dash rapide vers un pilier
    // ─────────────────────────────────────────────
    public void AttaqueSpe5()
    {
        GameObject cible = GetClosestStalactite();
        if (cible == null) return;
        StartCoroutine(AttaqueSpe5Routine(cible));
    }

    IEnumerator AttaqueSpe5Routine(GameObject cible)
    {
        isAttacking = true;

        if (cible == null)
        {
            isAttacking = false;
            yield break;
        }

        Vector3 dir = (cible.transform.position - transform.position).normalized;
        float dashSpeed = speed * 4f;
        float maxDashTime = 2f;
        float timer = 0f;

        while (timer < maxDashTime)
        {
            if (cible == null || !cible.activeSelf) break;

            float dist = Vector3.Distance(transform.position, cible.transform.position);
            if (dist <= 1f) break;

            transform.position += dir * dashSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        if (cible != null)
            cible.SetActive(false);

        if (player != null)
        {
            Vector3 dirShot = (player.transform.position - transform.position).normalized;

            if (projectileCount >= projectilePrefab.Count) projectileCount = 0;

            if (!projectilePrefab[projectileCount].activeSelf)
            {
                GameObject proj = projectilePrefab[projectileCount];
                proj.SetActive(true);
                proj.transform.position = transform.position;
                proj.transform.rotation = Quaternion.identity;
                proj.transform.forward = dirShot;
                proj.GetComponent<Rigidbody>().linearVelocity = dirShot * 10f;
                projectileCount++;
            }
        }

        cooldownDashPilier = cooldownEntreDashPilier;
        yield return new WaitForSeconds(0.3f);
        cooldownTimer = cooldownEntreAttaque;
        isAttacking = false;
    }

    // ─────────────────────────────────────────────
    // CHUTE STALACTITE PASSIVE
    // ─────────────────────────────────────────────
    public void ChuteStalactite()
    {
        if (isAttacking) return;
        StartCoroutine(ChuteStalactiteRoutine());
    }

    IEnumerator ChuteStalactiteRoutine()
    {
        isAttacking = true;

        if (zoneCount >= zones.Count) zoneCount = 0;

        Vector3 randomPos = transform.position + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));

        if (!zones[zoneCount].activeSelf)
        {
            positions[zoneCount] = randomPos;
            zones[zoneCount].SetActive(true);
            zones[zoneCount].transform.position = randomPos;
        }

        int usedZone = zoneCount;
        zoneCount++;
        if (zoneCount >= zones.Count) zoneCount = 0;

        yield return new WaitForSeconds(vitesseAttaque);

        if (StalactiteCount >= stalactitePrefab.Count) StalactiteCount = 0;

        if (!stalactitePrefab[StalactiteCount].activeSelf)
        {
            stalactitePrefab[StalactiteCount].SetActive(true);
            stalactitePrefab[StalactiteCount].transform.position = positions[usedZone];

            Collider[] hits = Physics.OverlapSphere(positions[usedZone], 1.5f, playerLayer);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    Debug.Log("Player touché par ChuteStalactite");
                    DealDammage();
                }
                    
            }

            StalactiteCount++;
        }

        zones[usedZone].SetActive(false);

        cooldownTimerStalactite = cooldownEntreAttaqueStalactite;
        isAttacking = false;
    }
    public void DealDammage()
    {
        playerHealth.takeDamage(attaque);
    }
}