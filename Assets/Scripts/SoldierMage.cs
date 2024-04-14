using System;
using UnityEngine;

public class SoldierMage : Soldier
{
    protected override void Attack(Soldier enemy)
    {
        var newPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        var towardsEnemy = enemy.transform.position - transform.position;
        // normally the mage would should behind the enemy because that's the current position
        // but assuming that the projectile will fly for 2 seconds, then we can compute their expected new position
        // var enemyPosition =  enemy.transform.position - enemy.GetComponent<Rigidbody>().velocity*5.0f;
        // nwm doesn't work
        var enemyPosition =  enemy.transform.position;
        var enemyDistance = (enemyPosition - transform.position).magnitude;

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
