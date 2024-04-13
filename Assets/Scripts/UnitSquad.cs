using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSquad : MonoBehaviour
{
    public string team;
    private HatType squadHat;
    private List<Soldier> squadMembers = new List<Soldier>();

    public void addSquadMember(Soldier soldier)
    {
        squadMembers.Add(soldier);
    }

    void Start()
    {
        var hatTypes = System.Enum.GetValues(typeof(HatType));
        HatType squadHat = (HatType)hatTypes.GetValue(Random.Range(0, hatTypes.Length));
    }

    public void SetHat(HatType hat)
    {
        squadHat = hat;
        foreach (var soldier in squadMembers)
        {
            soldier.SetHat(hat);
        }
    }

    public void InitSquad(int squadSize, GameObject unitPrefab, Vector3 position, string team)
    {
        for (int i = 0; i < squadSize; i++)
        {
            // TODO: calculate position based on squad size / formation
            var soldier = Instantiate(unitPrefab, position + new Vector3(2 * i, 0, 0), Quaternion.identity).GetComponent<Soldier>();
            soldier.transform.parent = transform;
            soldier.SetHat(squadHat);
            soldier.SetTeam(team);
            squadMembers.Add(soldier);
        }
    }
}
