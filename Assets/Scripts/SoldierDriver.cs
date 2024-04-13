using UnityEngine;
using UnityEngine.EventSystems;

public class SoldierDriver : MonoBehaviour
{
    public bool debugSpawnEnemies;

    void Start()
    {
        if (debugSpawnEnemies)
        {
            SpawnDebugEnemies();
        }

        EventManager.OnBattleStart += () =>
        {
            SpawnDebugEnemies();
        };
    }

    void SpawnDebugEnemies()
    {
        Spawn("B", "UnitBasic", new Vector3(3, 0, 3));
        Spawn("B", "UnitArcher", new Vector3(4, 0, 3));
        Spawn("B", "UnitArcher", new Vector3(5, 0, 3));
        Spawn("B", "UnitBasic", new Vector3(6, 0, 3));
        Spawn("B", "UnitBasic", new Vector3(7, 0, 3));
    }

    public void Spawn(string team, string unitType, Vector3 position)
    {
        GameObject unitPrefab = Resources.Load<GameObject>("Units/" + unitType);
        var soldier = Instantiate(unitPrefab, position, Quaternion.identity).GetComponent<Soldier>();
        soldier.SetTeam(team);
    }

    public void SpawnSquad(string team, string unitType, Vector3 position)
    {
        GameObject newGameObject = new GameObject("Squad/" + unitType);
        UnitSquad unitSquad = newGameObject.AddComponent<UnitSquad>();
        unitSquad.InitSquad(4, Resources.Load<GameObject>("Units/" + unitType), position, team);
    }
}
