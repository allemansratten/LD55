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

    void Start()
    {
        var hatTypes = System.Enum.GetValues(typeof(HatType));
        squadHat = (HatType)hatTypes.GetValue(Random.Range(0, hatTypes.Length));
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
        List<UnitDragHandler> dragHandlers = new();
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
            dragHandlers.Add(dragEvent);

            squadMembers.Add(soldier);
        }

        dragHandlers.ForEach(dragHandler =>
        {
            /// Save the start position of all squad members when the any one is clicked
            dragHandler.MouseDown += () =>
            {
                foreach (var soldier in squadMembers)
                {
                    soldier.GetComponent<UnitDragHandler>().SaveStartPos();
                }
            };

            /// Move all squad members when the any one is dragged
            dragHandler.MouseDragged += (Vector3 mouseWorldPos) =>
            {
                foreach (var soldier in squadMembers)
                {
                    soldier.transform.position = soldier.GetComponent<UnitDragHandler>().DragStartTransformPosition + mouseWorldPos;
                }
            };
        });
    }

    private void OnSoldierClicked()
    {
    }
}
