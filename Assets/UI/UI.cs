using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        UnitShopManager unitShopManager = GameObject.Find("Ground").GetComponent<UnitShopManager>();

        Button buttonStart = root.Q<Button>("button-start");
        Button buttonTeam = root.Q<Button>("button-team");

        buttonStart.clicked += () =>
        {
            EventManager.BattleStart();
        };

        buttonTeam.clicked += () =>
        {
            unitShopManager.SwitchTeam();
        };
    }
}
