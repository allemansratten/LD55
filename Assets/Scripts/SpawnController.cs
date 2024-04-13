using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnController : MonoBehaviour
{
    public GameObject guyPrefab;
    private bool canSpawn = false;
    // Start is called before the first frame update
    void Start()
    {

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
        if (canSpawn)
        {
            Instantiate(guyPrefab, position, Quaternion.identity);
        }
    }

    public void ToggleSpawning(bool canSpawn)
    {
        this.canSpawn = canSpawn;
    }
}
