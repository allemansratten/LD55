using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject guyPrefab;
    public GameObject teamToggle;

    public bool spawnRedTeam = true;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(guyPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        SetSpawnRedTeam(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpawnRedTeam(bool isRedTeam)
    {
        spawnRedTeam = isRedTeam;
        teamToggle.GetComponent<Image>().color = isRedTeam ? Color.red : Color.cyan;
    }
}
