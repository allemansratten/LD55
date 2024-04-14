using System;
using UnityEngine;
using UnityEngine.AI;

public class SoldierMelee : Soldier
{
    public override void SetHat(HatType hatType)
    {
        base.SetHat(hatType);
        if (hatType == HatType.HardHat)
        {
            // increase scale and health
            transform.localScale *= 1.5f;
            // TODO(vv): Distinguish max health and current health
            health *= 2;
        }
        else if (hatType == HatType.Horse)
        {
            // TODO(vv): Do this via a modifier?
            navMeshAgent = GetComponent<NavMeshAgent>();
            speed.BaseValue = navMeshAgent.speed;

            speed.BaseValue *= 2.0f;

            // This is needed because the method is called before Start(),
            // otherwise Start() would overwrite the speed value again.
            navMeshAgent.speed = speed.BaseValue;
        }
    }
}
