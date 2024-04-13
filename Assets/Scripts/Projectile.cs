using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public string team;
    public float damage = 25;

    void OnTriggerEnter(Collider collider)
    {
        var soldier = collider.gameObject.GetComponent<Soldier>();
        bool isSoldier = soldier != null;
        if (!isSoldier)
        {
            if (collider.gameObject.name == "Ground")
            {
                Destroy(gameObject);
            }
        }
        else if (soldier.Team != team)
        {
            soldier.Hurt(damage);
            Destroy(gameObject);
        }
    }
}
