using UnityEngine;
using UnityEngine.EventSystems;

public class SoldierDriver : MonoBehaviour
{
    private bool canSpawn = false;

    void Start()
    {
        Spawn("B", "Basic", new Vector3(-3, 0, 3));
        Spawn("B", "Biggon", new Vector3(-4, 0, 3));
        Spawn("B", "Biggon", new Vector3(-5, 0, 3));
        Spawn("B", "Basic", new Vector3(-6, 0, 3));
        Spawn("B", "Basic", new Vector3(-7, 0, 3));

        EventManager.OnBattleStart += () =>
        {
            Debug.Log("Battle started");
            this.canSpawn = false;
        };
    }

    public void Spawn(string team, string unitType, Vector3 position)
    {
        GameObject unitPrefab = Resources.Load<GameObject>("Units/Unit" + unitType);
        var soldier = Instantiate(unitPrefab, position, Quaternion.identity).GetComponent<Soldier>();
        soldier.team = team;

        soldier.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Units/Team" + team);
    }

    public UnitSquad SpawnSquad(string team, string unitType, Vector3 position)
    {
        return new UnitSquad(4, Resources.Load<GameObject>("Units/Unit" + unitType), position, team);
    }
}
