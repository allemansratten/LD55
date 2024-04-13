using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class SoldierMelee : Soldier
{
    void LandHit()
    {
        Debug.Log("Lang Hit!");
    }

    void OnTriggerEnter(Collider collider)
    {
        // TODO: just call super.OnTriggerEnter
        // don't go through any of this if we already have an enemy
        if (currentEnemy != null) return;

        var other = collider.gameObject.GetComponent<Soldier>();
        // we did not collide with enemy

        if (other is null || other.Team == team)
        {
            return;
        }
        else
        {
            currentEnemy = other;
        }

        // stop movement
        navMeshAgent.isStopped = true;

        // start shooting
        StartCoroutine("ShootEnemy");

    }

    IEnumerator ShootEnemy()
    {
        while (currentEnemy != null)
        {
            GameObject projectile = Instantiate(Resources.Load<GameObject>("Projectile"), transform.position + Vector3.up, Quaternion.identity);

            projectile.GetComponent<Rigidbody>().velocity = (currentEnemy.transform.position - transform.position).normalized * 10.0f;
            projectile.GetComponent<Projectile>().team = team;

            // fire every second
            yield return new WaitForSeconds(1.0f + Random.value);
        }
    }
}