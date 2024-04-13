using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnOnClick : MonoBehaviour
{
    private SoldierDriver soldierDriver;

    private bool canSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnBattleStart += () =>
        {
            Debug.Log("Battle started");
            this.canSpawn = false;
        };

        soldierDriver = GameObject.Find("Game Controller").GetComponent<SoldierDriver>();
    }

    // Update is called once per frame
    void Update()
    {

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
            spawnUnit(hit.point);
        }
    }

    private void spawnUnit(Vector3 position)
    {
        Debug.Log("Spawning unit");
        if (canSpawn)
        {
            soldierDriver.Spawn("A", "base", position);
        }
    }


    // void Spawn(string team, string unitType, Vector3 position)
    // {
    //     var soldier = Instantiate(soldierPrefab, position, Quaternion.identity).GetComponent<Soldier>();
    //     soldier.team = team;

    //     // change material on team
    //     foreach (var def in teamDefinitions)
    //     {
    //         if (def.team == team)
    //         {
    //             soldier.GetComponent<MeshRenderer>().material = def.material;
    //             break;
    //         }
    //     }

    //     // change scale on unit type
    //     foreach (var def in typeDefinitions)
    //     {
    //         if (def.unitType == unitType)
    //         {
    //             soldier.GetComponent<Transform>().localScale = def.scale;
    //             break;
    //         }
    //     }

    // }
}
