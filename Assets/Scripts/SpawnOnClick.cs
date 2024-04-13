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

    void OnMouseDown()
    {
        if (!this.canSpawn)
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
        if (canSpawn)
        {
            soldierDriver.SpawnSquad("A", "UnitBasic", position);
        }
    }
}
