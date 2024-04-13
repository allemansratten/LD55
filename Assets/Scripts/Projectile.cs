using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public string team;
    public float damage = 25;
    // If >0, do AoE damage on impact instead of damaging single units.
    public float areaOfEffect = 0;

    public GameObject aoeDamageIndicatorPrefab;

    void OnTriggerEnter(Collider collider)
    {
        var soldier = collider.gameObject.GetComponent<Soldier>();
        bool isSoldier = soldier != null;
        if (!isSoldier)
        {
            if (collider.gameObject.name == "Ground")
            {
                Land();
            }
        }
        else if (soldier.Team != team)
        {
            if (areaOfEffect == 0)
            {
                soldier.Hurt(damage);
            }
            Land();
        }
    }

    void Land()
    {
        if (areaOfEffect > 0)
        {
            DamageAreaOfEffect(transform.position);
        }
        Destroy(gameObject);
    }

    void DamageAreaOfEffect(Vector3 position)
    {
        // Find all colliders within the radius at the center
        Collider[] hitColliders = Physics.OverlapSphere(position, areaOfEffect);
        foreach (var hitCollider in hitColliders)
        {
            var soldier = hitCollider.gameObject.GetComponent<Soldier>();
            if (soldier != null && soldier.Team != team)
            {
                soldier.Hurt(damage);
                // also knock back (TODO: only for earth)
                var direction = (soldier.transform.position - position).normalized;
                soldier.gameObject.GetComponent<Rigidbody>().AddForce(direction * 100, ForceMode.Impulse);
            }
        }
        // For debugging, spawn a ring with this radius
        var ring = Instantiate(aoeDamageIndicatorPrefab);
        ring.transform.position = new Vector3(position.x, 0, position.z);
        ring.transform.localScale = new Vector3(areaOfEffect, 0.1f, areaOfEffect);
        ring.GetComponent<Collider>().enabled = false;
        Destroy(ring, 0.3f);
    }
}
