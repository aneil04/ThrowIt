using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjManager : MonoBehaviour, IPunOwnershipCallbacks
{
    public List<GameObject> allObjects = new List<GameObject>();
    public Vector2 xBounds;
    public Vector2 zBounds;
    public float yHeight;
    public float objDensity;
    float totalNumOfObjects;
    private PhotonView photonView;

    private int numOfPowerups = 0;
    private int numOfObjects = 0;
    float area;

    public GameObject powerupPrefab;
    public float powerupSpawnTime;
    private float spawnTime;
    public float powerupDensity;
    float totalNumOfPowerups;

    public bool isMenu;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        area = Mathf.Abs(xBounds.y - xBounds.x) * Mathf.Abs(zBounds.y - zBounds.x);
        totalNumOfObjects = objDensity * area;
        totalNumOfPowerups = powerupDensity * area;

        while (this.numOfObjects <= totalNumOfObjects)
        {
            spawnObj();
        }

        while (this.numOfPowerups <= totalNumOfPowerups && !isMenu)
        {
            spawnPowerup();
        }
    }

    void Update()
    {
        if (this.numOfPowerups < totalNumOfPowerups && !isMenu)
        {
            while (this.numOfPowerups <= totalNumOfPowerups)
            {
                spawnPowerup();
            }
        }
    }

    void spawnObj()
    {
        GameObject objToSpawn = allObjects[(int)Random.Range(0, allObjects.Count)];
        Vector3 spawnPos = new Vector3(Random.Range(xBounds.x, xBounds.y), yHeight, Random.Range(zBounds.x, zBounds.y));
        if (isMenu)
        {
            Instantiate(objToSpawn, spawnPos, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.InstantiateRoomObject(objToSpawn.name, spawnPos, Quaternion.identity);
        }
        this.numOfObjects++;
    }

    void spawnPowerup()
    {
        Vector3 spawnPos = new Vector3(Random.Range(xBounds.x, xBounds.y), 30, Random.Range(zBounds.x, zBounds.y));
        PhotonNetwork.InstantiateRoomObject(powerupPrefab.name, spawnPos, Quaternion.identity);
        this.numOfPowerups++;
    }

    public void decrementNumOfPowerup()
    {
        this.numOfPowerups--;
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("ownership requested");
        // throw new System.NotImplementedException();
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        throw new System.NotImplementedException();
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }
}
