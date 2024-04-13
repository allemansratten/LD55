using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonStart = root.Q<Button>("button-start");
        Button buttonTeam = root.Q<Button>("button-team");

        buttonStart.clicked += () =>
        {
            Debug.Log("Start button clicked");
            EventManager.BattleStart();
        };

        buttonTeam.clicked += () =>
        {
            Debug.Log("Team button clicked");
        };
    }
}
