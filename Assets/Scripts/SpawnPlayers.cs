using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SpawnPlayers : MonoBehaviourPun
{
    public GameObject playerPrefab;
    public GameObject agentPrefab;

    public Vector2 xBounds;
    public Vector2 zBounds;
    public float yHeight;

    public int numOfAgents;

    void Start()
    {
        Vector3 spawnPos = new Vector3(Random.Range(xBounds.x, xBounds.y), yHeight, Random.Range(zBounds.x, zBounds.y));
        object[] customPlayerData = { };
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, Quaternion.identity, 0, customPlayerData);

        for (int x = 0; x < numOfAgents; x++)
        {
            spawnAgent();
        }
    }

    private void spawnAgent()
    {
        //instantiate here
        Vector3 spawnPos = new Vector3(Random.Range(xBounds.x, xBounds.y), yHeight, Random.Range(zBounds.x, zBounds.y));
        object[] customPlayerData = { };
        GameObject player = PhotonNetwork.Instantiate(agentPrefab.name, spawnPos, Quaternion.identity, 0, customPlayerData);
    }

    public void agentDied()
    {
        //when agent has died, wait a set amount of time and spawn a new agent 
        StartCoroutine("delaySpawn");
    }

    IEnumerator delaySpawn()
    {
        yield return new WaitForSeconds(5f);
        spawnAgent();
    }
}
