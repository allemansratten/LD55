using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    public string team;
    [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        StartCoroutine("FindNewEnemy");
    }

    IEnumerator FindNewEnemy() {
        while(true) {
            var my_pos = GetComponent<Transform>().position;
            var soldiers = FindObjectsOfType<Soldier>();

            Debug.Log(soldiers.Length);

            float? min_dist = null;
            Soldier? min_soldier = null;


            foreach(var soldier in soldiers) {
                // skip our team
                if (soldier.team == team) {
                    continue;
                }

                var soldier_pos = soldier.GetComponent<Transform>().position;
                var soldier_dist = Vector3.Distance(my_pos, soldier_pos);
                if (min_dist is null || soldier_dist < min_dist) {
                    min_dist = soldier_dist;
                    min_soldier = soldier;
                }
            }

            // repeatedly find nearest soldier, if it exists
            if(!(min_soldier is null)) {
                navMeshAgent.destination = min_soldier.GetComponent<Transform>().position;
            }

            // repeat every second
            yield return new WaitForSeconds(1.0f);
        }
    }

    void OnTriggerEnter(Collider collider) {
        var enemy = collider.gameObject.GetComponent<Soldier>();
        // we did not collide with enemy
        if (enemy is null || enemy.team == team) {
            return;
        }

        if(Random.value > 0.5f) {
            Destroy(this.gameObject);
        }
    }
}
