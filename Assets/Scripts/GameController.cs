using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool spawnRedTeam = true;

    // Start is called before the first frame update
    void Start()
    {
        SetSpawnRedTeam(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpawnRedTeam(bool isRedTeam)
    {
        spawnRedTeam = isRedTeam;
    }
}
