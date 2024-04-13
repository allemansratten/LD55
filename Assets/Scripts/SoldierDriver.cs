using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierDriver : MonoBehaviour
{
    public GameObject soldierPrefab;
    public List<TeamDefinition> teamDefinitions;
    public List<TypeDefinition> typeDefinitions;

   [System.Serializable]
    public class TeamDefinition {
       public string team;
       public Material material;
   }

   [System.Serializable]
    public class TypeDefinition {
       public string unitType;
       public Vector3 scale;
   }

    void Spawn(string team, string unitType, Vector3 position) {
        var soldier = Instantiate(soldierPrefab, position, Quaternion.identity).GetComponent<Soldier>();
        soldier.team = team;

        // change material on team
        foreach(var def in teamDefinitions) {
            if(def.team == team) {
                soldier.GetComponent<MeshRenderer>().material = def.material;
                break;
            }
        }
        
        // change scale on unit type
        foreach(var def in typeDefinitions) {
            if(def.unitType == unitType) {
                soldier.GetComponent<Transform>().localScale = def.scale;
                break;
            }
        }

    }
    
    void Start() {
        Spawn("A", "base", new Vector3(33, 0, -20));
        Spawn("A", "base", new Vector3(36, 0, -20));
        Spawn("A", "base", new Vector3(39, 0, -20));
        Spawn("A", "base", new Vector3(42, 0, -20));
        Spawn("A", "base", new Vector3(45, 0, -20));

        
        Spawn("B", "biggie", new Vector3(33, 0, 10));
        Spawn("B", "biggie", new Vector3(36, 0, 10));
        Spawn("B", "base", new Vector3(39, 0, 10));
    }
}
