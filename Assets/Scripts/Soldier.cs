using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    public string team;
    [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;

    public HatType hatType = HatType.NoHat;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        StartCoroutine("FindNewEnemy");

        SetHat(RandomHat());
    }

    // For testing purposes.
    HatType RandomHat()
    {
        var hatTypes = System.Enum.GetValues(typeof(HatType));
        return (HatType)hatTypes.GetValue(Random.Range(0, hatTypes.Length));
    }

    IEnumerator FindNewEnemy()
    {
        while (true)
        {
            var my_pos = GetComponent<Transform>().position;
            var soldiers = FindObjectsOfType<Soldier>();

            float? min_dist = null;
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
            }

            // repeat every second
            yield return new WaitForSeconds(1.0f);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        var enemy = collider.gameObject.GetComponent<Soldier>();
        // we did not collide with enemy
        if (enemy is null || enemy.team == team)
        {
            return;
        }

        if (Random.value > 0.5f)
        {
            Destroy(this.gameObject);
        }
    }

    void SetHat(HatType hatType)
    {
        this.hatType = hatType;
        GameObject child = transform.GetChild(0).gameObject;
        MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
        meshRenderer.material = Resources.Load<Material>("Hats/" + hatType.ToString());
    }
}
