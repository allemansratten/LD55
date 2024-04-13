using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSquad : MonoBehaviour
{
    public string team;
    private List<Soldier> squadMembers = new List<Soldier>();

    public void addSquadMember(Soldier soldier)
    {
        squadMembers.Add(soldier);
    }

    public UnitSquad(int squadSize, GameObject unitPrefab, Vector3 position, string team)
    {
        for (int i = 0; i < squadSize; i++)
        {
            // TODO: calculate position based on squad size / formation
            var soldier = Instantiate(unitPrefab, position + new Vector3(2 * i, 0, 0), Quaternion.identity).GetComponent<Soldier>();
            soldier.team = team;
            soldier.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Units/Team" + team);
            squadMembers.Add(soldier);
        }
    }
}
