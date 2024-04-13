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
        var hatTypes = System.Enum.GetValues(typeof(HatType));
        HatType squadHat = (HatType)hatTypes.GetValue(Random.Range(0, hatTypes.Length));

        for (int i = 0; i < squadSize; i++)
        {
            // TODO: calculate position based on squad size / formation
            var soldier = Instantiate(unitPrefab, position + new Vector3(2 * i, 0, 0), Quaternion.identity).GetComponent<Soldier>();
            soldier.SetHat(squadHat);
            soldier.SetTeam(team);
            squadMembers.Add(soldier);
        }
    }
}
