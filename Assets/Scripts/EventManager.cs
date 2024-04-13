using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    // All actions that can be subscibed to
    // subscribe using EventManager.OnBattleStart += () => { /* code here */ };
    public static event Action OnBattleStart;

    // These handlers invoke the events
    public static void BattleStart()
    {
        OnBattleStart?.Invoke();
    }
}
