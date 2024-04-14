using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SoldierArcher : Soldier
{
    public int nAttacksOnSameTarget = 0;
    private Soldier lastEnemy;

    private int fireAttackStrongAfterNShots = 3;

    protected override void Attack(Soldier enemy)
    {
        if (enemy == lastEnemy)
        {
            nAttacksOnSameTarget++;
        }
        else
        {
            nAttacksOnSameTarget = 0;
            lastEnemy = enemy;
        }

        var newPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        var towardsEnemy = (enemy.transform.position - transform.position).normalized;
        GameObject projectileObject = Instantiate(projectilePrefab, newPos, Quaternion.LookRotation(towardsEnemy));
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        float damage = projectileDamage;
        Color color = new Color(0.2f, 0.2f, 0.1f);

        if (hatType == HatType.Horse)
        {
            // set to red color
            color = Color.red;

            if (nAttacksOnSameTarget >= fireAttackStrongAfterNShots)
            {
                damage *= 2;
                // scale projectile 2x
                projectileObject.transform.localScale *= 2;
            }
        }
        else if (hatType == HatType.WizardHat)
        {
            projectile.slowDuration = 2;
            color = Color.blue;
        }
        else if (hatType == HatType.HardHat)
        {
            // TODO: ?
        }
        else
        {
            Debug.LogWarning("Unknown hat type: " + hatType);
        }

        projectileObject.GetComponent<Rigidbody>().velocity = towardsEnemy * projectileSpeed;
        projectile.team = team;
        projectile.damage = damage;

        foreach (var renderer in projectileObject.GetComponentsInChildren<Renderer>())
        {

            renderer.material.color = color;
        }
    }
}
