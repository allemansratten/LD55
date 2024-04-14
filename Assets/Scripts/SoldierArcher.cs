using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SoldierArcher : Soldier
{
    protected override void Attack(Soldier enemy)
    {
        Debug.Log("Archer attack!");
        var newPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        var towardsEnemy = (enemy.transform.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, newPos, Quaternion.LookRotation(towardsEnemy));

        projectile.GetComponent<Rigidbody>().velocity = towardsEnemy * projectileSpeed;
        projectile.GetComponent<Projectile>().team = team;
        projectile.GetComponent<Projectile>().damage = projectileDamage;
    }
}
