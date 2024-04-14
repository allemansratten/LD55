using System;
using UnityEngine;

public class SoldierMage : Soldier
{
    protected override void Attack(Soldier enemy)
    {
        var newPos = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
        var towardsEnemy = enemy.transform.position - transform.position;
        var enemyDistance = (enemy.transform.position - transform.position).magnitude;
        towardsEnemy = new Vector3(towardsEnemy.x, 0, towardsEnemy.z).normalized;
        // this should have 45% angle
        towardsEnemy = new Vector3(towardsEnemy.x, 1, towardsEnemy.z);

        GameObject projectile = Instantiate(projectilePrefab, newPos, Quaternion.LookRotation(towardsEnemy));

        // magic /2 constant
        var actualProjectileSpeed = (float) Math.Sqrt(10*enemyDistance/2);
        projectile.GetComponent<Rigidbody>().velocity = towardsEnemy * actualProjectileSpeed;

        projectile.GetComponent<Projectile>().team = team;
        projectile.GetComponent<Projectile>().damage = projectileDamage;
    }
}
