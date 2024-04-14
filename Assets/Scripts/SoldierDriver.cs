using UnityEngine;
using UnityEngine.EventSystems;

public class SoldierDriver : MonoBehaviour
{
    private bool isBattleStarted = false;
    public bool IsBattleStarted { get => isBattleStarted; }

    void Start()
    {
        EventManager.OnBattleStart += () =>
        {
            foreach (var soldier in FindObjectsOfType<Soldier>())
            {
                soldier.enabled = true;
                isBattleStarted = true;
            }
        };
    }

    void SpawnDebugEnemies()
    {
        Spawn("B", "UnitMelee", new Vector3(3, 0, 3));
        //Spawn("B", "UnitArcher", new Vector3(4, 0, 3));
        //Spawn("B", "UnitArcher", new Vector3(5, 0, 3));
        //Spawn("B", "UnitMelee", new Vector3(6, 0, 3));
        //Spawn("B", "UnitMelee", new Vector3(7, 0, 3));
    }

    public void Spawn(string team, string unitType, Vector3 position)
    {
        if (unitType == "UnitBasic")
        {
            throw new System.Exception("Spawning UnitBasic is not allowed. Use UnitMelee.");
        }
        GameObject unitPrefab = Resources.Load<GameObject>("Units/" + unitType);
        var soldier = Instantiate(unitPrefab, position, Quaternion.identity).GetComponent<Soldier>();
        soldier.SetTeam(team);
    }

    public void SpawnSquad(string team, string unitType, Vector3 position)
    {
        if (unitType == "UnitBasic")
        {
            throw new System.Exception("Spawning UnitBasic is not allowed. Use UnitMelee.");
        }
        GameObject newGameObject = new GameObject("Squad/" + unitType);
        UnitSquad unitSquad = newGameObject.AddComponent<UnitSquad>();
        GameObject unit = Resources.Load<GameObject>("Units/" + unitType);
        unitSquad.InitSquad(unit.GetComponent<Soldier>().unitsPerSquad, unit, position, team);
    }
}
