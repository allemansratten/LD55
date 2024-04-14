using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusText : MonoBehaviour
{
    private Soldier soldier;
    private TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameObject.Find("Dynamic Text Canvas").transform, false);
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void SetSoldier(Soldier soldier)
    {
        this.soldier = soldier;
    }

    // Update is called once per frame
    void Update()
    {
        if (soldier == null)
        {
            Destroy(gameObject);
            return;
        }
        string statusTextString = "";
        foreach (var statusEffect in soldier.statusEffects)
        {
            statusTextString += statusEffect.Name + "\n";
        }
        // position status text at my position
        transform.position = Camera.main.WorldToScreenPoint(soldier.gameObject.transform.position + new Vector3(0, 0, 0));
        textMesh.text = statusTextString;
    }
}
