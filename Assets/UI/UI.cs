using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private bool isBattleStarted = false;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        UnitShopManager unitShopManager = GameObject.Find("Ground").GetComponent<UnitShopManager>();

        Button buttonStart = root.Q<Button>("button-start");

        buttonStart.clicked += () =>
        {
            if (!isBattleStarted)
            {
                buttonStart.text = "Reset";
                EventManager.BattleStart();
                isBattleStarted = true;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        };
    }
}
