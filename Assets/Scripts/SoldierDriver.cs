using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoldierDriver : MonoBehaviour
{

    private bool canSpawn = false;

    public GameObject soldierPrefab;
    public List<TeamDefinition> teamDefinitions;
    public List<TypeDefinition> typeDefinitions;

    [System.Serializable]
    public class TeamDefinition
    {
        public string team;
        public Material material;
    }

    [System.Serializable]
    public class TypeDefinition
    {
        public string unitType;
        public Vector3 scale;
    }

    public void Spawn(string team, string unitType, Vector3 position)
    {
        var soldier = Instantiate(soldierPrefab, position, Quaternion.identity).GetComponent<Soldier>();
        soldier.team = team;

        // change material on team
        foreach (var def in teamDefinitions)
        {
            if (def.team == team)
            {
                soldier.GetComponent<MeshRenderer>().material = def.material;
                break;
            }
        }

        // change scale on unit type
        foreach (var def in typeDefinitions)
        {
            if (def.unitType == unitType)
            {
                soldier.GetComponent<Transform>().localScale = def.scale;
                break;
            }
        }

    }

    void Start()
    {
        Spawn("A", "base", new Vector3(3, 0, 0));

        Spawn("B", "base", new Vector3(-3, 0, 3));
        Spawn("B", "base", new Vector3(-4, 0, 3));
        Spawn("B", "base", new Vector3(-5, 0, 3));
        Spawn("B", "base", new Vector3(-6, 0, 3));
        Spawn("B", "base", new Vector3(-7, 0, 3));

        EventManager.OnBattleStart += () =>
        {
            Debug.Log("Battle started");
            this.canSpawn = false;
        };
    }

    void OnMouseDown()
    {
        if (!this.canSpawn || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Spawn("A", "base", hit.point);
        }
    }
}
