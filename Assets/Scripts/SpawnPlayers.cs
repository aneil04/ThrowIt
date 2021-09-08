using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SpawnPlayers : MonoBehaviourPun
{
    public GameObject playerPrefab;

    public Vector2 xBounds;
    public Vector2 zBounds;
    public float yHeight;

    void Start()
    {
        Vector3 spawnPos = new Vector3(Random.Range(xBounds.x, xBounds.y), yHeight, Random.Range(zBounds.x, zBounds.y));

        object[] customPlayerData = {  };
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity, 0, customPlayerData); 
    }
}
