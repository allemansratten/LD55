using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    protected string team;
    // Readonly for the public
    public string Team { get => team; }
    protected NavMeshAgent navMeshAgent;

    public float health;

    public HatType hatType = HatType.NoHat;

    // Values to be overridden in the editor for different prefabs
    public float engageDistance = 25;
    public float projectileSpeed = 10;
    public float projectileDamage = 25;
    public int unitsPerSquad = 4;
    public GameObject projectilePrefab;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        StartCoroutine("FindNewEnemy");
    }

    IEnumerator FindNewEnemy()
    {
        while (true)
        {
            if (engagedEnemy == null)
            {
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
                    StartCoroutine("StartShooting");
                }
                else if (min_soldier != null)
                {
                    navMeshAgent.destination = min_soldier.GetComponent<Transform>().position;
                    navMeshAgent.isStopped = false;
                    Debug.Log(min_dist);

                }
            }

            // repeat every second
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator StartShooting()
    {
        while (engagedEnemy != null)
        {
            var newPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            var towardsEnemy = (engagedEnemy.transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, newPos, Quaternion.LookRotation(towardsEnemy));

            projectile.GetComponent<Rigidbody>().velocity = towardsEnemy * projectileSpeed;
            projectile.GetComponent<Projectile>().team = team;
            projectile.GetComponent<Projectile>().damage = projectileDamage;

            // fire every second
            yield return new WaitForSeconds(1.0f + Random.value);
        }
    }

    protected Soldier? engagedEnemy = null;

    public void SetHat(HatType hatType)
    {
        this.hatType = hatType;

        GameObject child = transform.Find("Hat").gameObject;
        MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
        meshRenderer.material = Resources.Load<Material>("Hats/" + hatType.ToString());
    }

    public void SetTeam(string team)
    {
        this.team = team;

        GameObject child = transform.Find("Beta_Surface").gameObject;
        SkinnedMeshRenderer meshRenderer = child.GetComponent<SkinnedMeshRenderer>();
        meshRenderer.material = Resources.Load<Material>("Units/Team" + team);

    }

    public void Hurt(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
