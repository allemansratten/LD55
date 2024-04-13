using UnityEngine;
using UnityEngine.UIElements;

public class UnitShopManager : MonoBehaviour
{
    private SoldierDriver soldierDriver;
    private string unitBeingSpawned;
    private bool isSpawningEnabled = true;
    // TODO: replace with actual unit list from the game
    private readonly string[] tempUnitList = {
        "UnitBasic",
        "UnitBiggon"
    };

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

    private void CreateGuiElements()
    {
        UIDocument uiDocument = FindObjectOfType<UIDocument>();

        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found in the scene.");
            return;

        }
        // Get the root visual element of the UI
        VisualElement root = uiDocument.rootVisualElement;

        foreach (var unit in tempUnitList)
        {
            Button button = new()
            {
                name = unit,
                text = unit
            };
            button.clicked += () => UnitSelected(unit);
            root.Add(button);
        }
    }

    void OnMouseDown()
    {
        if (!isSpawningEnabled || unitBeingSpawned == null)
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
        unitBeingSpawned = unit;
    }

    private void SpawnUnit(Vector3 position)
    {
        if (isSpawningEnabled && unitBeingSpawned != null)
        {
            soldierDriver.SpawnSquad("A", unitBeingSpawned, position);
        }
        unitBeingSpawned = null;
    }
}
