using System.Collections.Generic;
using UnityEngine;

public class UnitSquad : MonoBehaviour
{
    public string team;
    private HatType squadHat;
    private readonly List<Soldier> squadMembers = new();

    public void AddSquadMember(Soldier soldier)
    {
        squadMembers.Add(soldier);
    }

    public void Start()
    {
        var hatTypes = System.Enum.GetValues(typeof(HatType));
        squadHat = (HatType)hatTypes.GetValue(Random.Range(0, hatTypes.Length));
        EventManager.OnBattleStart += RemoveSquadDragHandlers;
        EventManager.OnBattleEnd += AddSquadDragHandlers;
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
            var clickEvent = soldier.gameObject.AddComponent<ClickableUnit>();
            clickEvent.MouseDown += OnSoldierClicked;
            var dragEvent = soldier.gameObject.AddComponent<UnitDragHandler>();

            squadMembers.Add(soldier);
        }

        AddSquadDragHandlers();
    }

    private void AddSquadDragHandlers()
    {
        squadMembers.ForEach(soldier =>
        {
            var dragHandler = soldier.GetComponent<UnitDragHandler>();
            dragHandler.MouseDown += SaveStartPos;
            dragHandler.MouseDragged += DragSquadUnits;
        });
    }

    private void RemoveSquadDragHandlers()
    {
        squadMembers.ForEach(soldier =>
        {
            var dragHandler = soldier.GetComponent<UnitDragHandler>();
            dragHandler.MouseDown -= SaveStartPos;
            dragHandler.MouseDragged -= DragSquadUnits;
        });
    }

    /// Save the start position of all squad members when the any one is clicked
    private void SaveStartPos()
    {
        foreach (var soldierToHandle in squadMembers)
        {
            soldierToHandle.GetComponent<UnitDragHandler>().SaveStartPos();
        }
    }

    /// Move all squad members when the any one is dragged
    private void DragSquadUnits(Vector3 mouseWorldPos)
    {
        foreach (var soldier in squadMembers)
        {
            soldier.transform.position = soldier.GetComponent<UnitDragHandler>().DragStartTransformPosition + mouseWorldPos;
        }
    }

    private void OnSoldierClicked()
    {
    }
}
