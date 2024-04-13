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

    void Update() {
        attackAnimation.Play();
        Debug.Log("STARTING");
        Debug.Log(attackAnimation);
        Debug.Log(attackAnimation.GetClipCount());
        Debug.Log(attackAnimation.clip);
    }

    void LandHit() {
        Debug.Log("Lang Hit!");
    }
}