using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitShopManager : MonoBehaviour
{
    private SoldierDriver soldierDriver;
    private string selectedUnitType;
    private bool isSpawningEnabled = true;
    private string teamToSpawn = "A";

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnBattleStart += () =>
        {
            isSpawningEnabled = false;
        };

        EventManager.OnBattleEnd += () =>
        {
            isSpawningEnabled = true;
        };

        soldierDriver = GameObject.Find("Game Controller").GetComponent<SoldierDriver>();
        CreateGuiElements();
    }

    private List<Button> unitButtons = new List<Button>();
    private void CreateGuiElements()
    {
        UIDocument uiDocument = FindObjectOfType<UIDocument>();

        var _unitButtons = uiDocument.rootVisualElement.Q("unit-shop").Children();
        foreach (var child in _unitButtons)
        {
            var button = child as Button;
            unitButtons.Add(button);
            button.clicked += () => {
                // clear all tint
                foreach(var child in unitButtons) {
                    child.style.unityBackgroundImageTintColor = Color.white;
                }
                // add self tint
                button.style.unityBackgroundImageTintColor = Color.green;
                UnitSelected(button.text);
            };
        }

        var teamButton = uiDocument.rootVisualElement.Q<Button>("button-team");
        teamButton.clicked += () =>
        {
            teamToSpawn = teamToSpawn == "A" ? "B" : "A";
            teamButton.text = "Team " + teamToSpawn;
        };
    }

    void OnMouseDown()
    {
        if (!isSpawningEnabled || selectedUnitType == null)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            SpawnUnit(hit.point);
        }
    }

    private void UnitSelected(string unit)
    {
        Debug.Log("Unit selected: " + unit);
        selectedUnitType = "Unit" + unit;
    }

    private void SpawnUnit(Vector3 position)
    {
        if (isSpawningEnabled && selectedUnitType != null)
        {
            soldierDriver.SpawnSquad(teamToSpawn, selectedUnitType, position);
        }
    }
}
