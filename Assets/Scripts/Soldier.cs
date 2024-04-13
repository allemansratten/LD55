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
    private Transform movePositionTransform;
    protected NavMeshAgent navMeshAgent;

    public float health;

    public HatType hatType = HatType.NoHat;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        StartCoroutine("FindNewEnemy");
    }

    IEnumerator FindNewEnemy()
    {
        while (true)
        {
            if(currentEnemy is null) {
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
                if (!(min_soldier is null))
                {
                    navMeshAgent.destination = min_soldier.GetComponent<Transform>().position;
                    navMeshAgent.isStopped = false;
                }
            }

            // repeat every second
            yield return new WaitForSeconds(1.0f);
        }
    }

    protected Soldier? currentEnemy = null;
    void OnTriggerEnter(Collider collider)
    {

        if(collider.gameObject.tag == "projectile") {
            health -= 20;
            if (health <= 0) {
                Destroy(gameObject);
            }
        }

        var other = collider.gameObject.GetComponent<Soldier>();
        // we did not collide with enemy
        if (other is null || other.team == team)
        {
            return;
        } else {
            currentEnemy = other;
        }

        if(Random.value > 0.5) {
            Destroy(gameObject);
            // gameObject.GetComponent<Soldier>().Hurt(25);
        }
    }

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

    public void Hurt(float damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
