using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    // TODO: maybe change the bounds to be a list of spawn points
    public Vector2 xBounds;
    public Vector2 zBounds;
    public float yHeight;
    void Start()
    {
        //spawn the player
        Vector3 spawnPos = new Vector3(Random.Range(xBounds.x, xBounds.y), yHeight, Random.Range(zBounds.x, zBounds.y));

        // string playerName = getPlayerName();

        object[] customPlayerData = {  };
        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity, 0, customPlayerData);
    }
    // public string getPlayerName()
    // {
    //     // string name = GameObject.FindGameObjectsWithTag("Persist")[0].GetComponent<PlayerName>().getName();
    //     // return name;
    // }
}
