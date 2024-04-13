using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public string team;
    void OnTriggerEnter(Collider collider)
    {
        var soldier = collider.gameObject.GetComponent<Soldier>();
        if(soldier is null) {
            Destroy(gameObject);
        } else if(soldier.Team != team) {
            soldier.Hurt(25);
            Destroy(gameObject);
        }   
    }
}
