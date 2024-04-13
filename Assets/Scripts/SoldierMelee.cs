using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMelee : Soldier
{
    private Animation attackAnimation;
    void Start()
    {
        attackAnimation = GetComponent<Animation>();
    }

    #pragma warning disable CS8632
    private Soldier? currentTarget = null;

    void OnTriggerEnter(Collider collider)
    {
        currentTarget = collider.gameObject.GetComponent<Soldier>();
        // we did not collide with enemy
        if (currentTarget is null || currentTarget.team == team)
        {
            currentTarget = null;
            return;
        } else {
        // attackAnimation.Play();
        }

    }

    void Update() {
        attackAnimation.Play();
        Debug.Log("STARTING");
        Debug.Log(attackAnimation);
        Debug.Log(attackAnimation.GetClipCount());
        Debug.Log(attackAnimation.clip);
    }

    void LandHit() {
        // TODO: start fight animation and then subtract health
        if(!(currentTarget is null)) {
            Destroy(currentTarget);
        }
        Debug.Log("Lang Hit!");
    }
}