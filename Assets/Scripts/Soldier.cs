using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    protected string team;
    // Readonly for the public
    public string Team { get => team; }
    protected NavMeshAgent navMeshAgent;

    public float health;

    public HatType hatType;

    Animator animator;
    Rigidbody soldierRigidbody;
    SoldierDriver soldierDriver;

    Vector3 velocity;
    // public so that StatusText can access it, but to add elements, use AddStatusEffect().
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
    TextMeshProUGUI statusText;
    public float pathingCooldown = 0;
    float previousRotation;
    float previousAngularVelocity;
    float smoothingValue = 0.1f;


    // Values to be overridden in the editor for different prefabs
    public float engageDistance = 25;
    public float projectileSpeed = 10;
    public float projectileDamage = 25;
    public int unitsPerSquad = 4;
    public GameObject projectilePrefab;
    public Material outlinerMaterial;
    public Material teamAMaterial;
    public Material teamBMaterial;
    public TextMeshProUGUI statusTextPrefab;

    // variables modifiable by status effects
    private Modifiable<float> speed = new(float.NaN);  // dummy value, overridden in Start()
    private Modifiable<float> shotCooldown = new(1.0f);
    private GameObject currentHat = null;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        // set desired speed based on nav mesh agent speed
        speed.BaseValue = navMeshAgent.speed;

        animator = GetComponent<Animator>();
        soldierRigidbody = GetComponent<Rigidbody>();
        soldierDriver = GameObject.Find("Game Controller").GetComponent<SoldierDriver>();

        // create status text and add it to canvas
        statusText = Instantiate(statusTextPrefab);
        statusText.GetComponent<StatusText>().SetSoldier(this);
    }

    // Can be overridden in subclass
    protected virtual void Attack(Soldier enemy)
    {
        var newPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        var towardsEnemy = (enemy.transform.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, newPos, Quaternion.LookRotation(towardsEnemy));

        projectile.GetComponent<Rigidbody>().velocity = towardsEnemy * projectileSpeed;
        projectile.GetComponent<Projectile>().team = team;
        projectile.GetComponent<Projectile>().damage = projectileDamage;
    }

    IEnumerator StartShooting()
    {
        while (engagedEnemy != null)
        {
            animator.SetTrigger("Shoot");
            Attack(engagedEnemy);

            // fire every second
            yield return new WaitForSeconds((1.0f + Random.value) * shotCooldown.Value);

        }
    }

    // YOLO
#pragma warning disable CS8632
    protected Soldier? engagedEnemy = null;

    public void SetHat(HatType hatType)
    {
        Debug.Log("Setting hat to: " + hatType);
        this.hatType = hatType;
        Destroy(currentHat);
        if (hatType == HatType.NoHat)
        {
            return;
        }

        GameObject hatPrefab = Resources.Load<GameObject>("Hats/" + hatType.ToString());
        if (hatPrefab == null)
        {
            Debug.LogWarning("Hat prefab not found for hat type: " + hatType);
            return;
        }

        SkinnedMeshRenderer meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        GameObject hat = Instantiate(hatPrefab, transform);
        hat.transform.localPosition = new Vector3(0, 0.1f, 0);
        currentHat = hat;
        GameObject headBone = FindBoneByName("mixamorig:Head", meshRenderer);
        if (headBone != null)
        {
            hat.transform.parent = headBone.transform;
            hat.transform.localPosition = headBone.transform.localPosition + new Vector3(0, 0.1f, 0);
        }
    }

    private GameObject FindBoneByName(string name, SkinnedMeshRenderer skinnedMeshRenderer)
    {
        foreach (var bone in skinnedMeshRenderer.bones)
        {
            if (bone.name == name)
            {
                return bone.gameObject;
            }
        }
        return null;
    }

    public void SetTeam(string team)
    {
        this.team = team;

        GameObject child = transform.Find("Beta_Surface").gameObject;
        SkinnedMeshRenderer meshRenderer = child.GetComponent<SkinnedMeshRenderer>();

        var materials = new List<Material>
        {
            team == "A" ? teamAMaterial : teamBMaterial,
            outlinerMaterial
        };
        meshRenderer.SetMaterials(materials);
    }

    public void Hurt(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (!soldierDriver.IsBattleStarted) return;

        //predava info animatoru, navMeshAgent neumi angular velocity, pocitam ho sam.
        animator.SetFloat("speed", navMeshAgent.velocity.magnitude);
        // Calculate current rotation
        float currentRotation = soldierRigidbody.rotation.eulerAngles.y;

        // Calculate rotation change from previous frame
        float rotationChange = currentRotation - previousRotation;


        float angleInRadians = rotationChange * Mathf.Deg2Rad;

        // Calculate angular velocity in radians per second
        float angularVelocity = angleInRadians / Time.deltaTime;

        animator.SetFloat("rotation", angularVelocity * smoothingValue + previousAngularVelocity * (1 - smoothingValue));
        // Update previousRotation for next frame
        previousAngularVelocity = angularVelocity * smoothingValue + previousAngularVelocity * (1 - smoothingValue);
        previousRotation = currentRotation;

        // Now you have the angular velocity in radians per second


        pathingCooldown -= Time.deltaTime;
        if (pathingCooldown <= 0)
        {
            UpdatePathing();
            // repeat every second
            pathingCooldown = 1;
        }

        UpdateStatusEffects();
    }

    void UpdatePathing()
    {
        if (engagedEnemy != null)
        {
            return;
        }
        var my_pos = GetComponent<Transform>().position;
        var soldiers = FindObjectsOfType<Soldier>();

        float? min_dist = null;
        // YOLO
#pragma warning disable CS8632
        Soldier? min_soldier = null;

        foreach (var soldier in soldiers)
        {
            // skip our team
            if (soldier.team == team)
            {
                continue;
            }

            var soldier_pos = soldier.GetComponent<Transform>().position;
            var soldier_dist = Vector3.Distance(my_pos, soldier_pos);
            if (min_dist is null || soldier_dist < min_dist)
            {
                min_dist = soldier_dist;
                min_soldier = soldier;

            }
        }

        // repeatedly find nearest soldier, if it exists
        if (min_dist != null && min_dist < engageDistance)
        {
            engagedEnemy = min_soldier;
            navMeshAgent.isStopped = true;

            StartCoroutine(nameof(StartShooting));

            animator.SetBool("IsMoving", false);

        }
        else if (min_soldier != null)
        {
            navMeshAgent.destination = min_soldier.GetComponent<Transform>().position;
            navMeshAgent.isStopped = false;
            animator.SetBool("IsMoving", true);

        }
    }

    void UpdateStatusEffects()
    {
        speed.Reset();
        shotCooldown.Reset();

        // Go backwards so we can remove elements without affecting the loop
        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {
            if (!statusEffects[i].Update(Time.deltaTime))
            {
                statusEffects.RemoveAt(i);
                continue;
            }

            var statusEffect = statusEffects[i];
            if (statusEffect.Name == "Slow")
            {
                speed.Value *= 0.5f;
                shotCooldown.Value *= 2.0f;
            }
        }

        navMeshAgent.speed = speed.Value;
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        // if a status effect with the same name is already applied, reset its duration
        foreach (var effect in statusEffects)
        {
            if (effect.Name == statusEffect.Name)
            {
                effect.Refresh(statusEffect.MaxDuration);
                return;
            }
        }
        statusEffects.Add(statusEffect);
    }
}
