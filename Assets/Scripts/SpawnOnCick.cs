using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnCick : MonoBehaviour
{
    public GameObject guyPrefab;
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            // instantiate the object at the click position
            Instantiate(guyPrefab, hit.point, Quaternion.identity);
        }
    }
}
