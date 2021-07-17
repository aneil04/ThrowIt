using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjManager : MonoBehaviour
{
    public List<GameObject> allObjects = new List<GameObject>();
    public List<GameObject> objectsInScene = new List<GameObject>();
    public Vector2 xBounds;
    public Vector2 zBounds;
    public float yHeight;
    public float objDensity; //how many objects per unit to spawn at the beginning of the game
    public float totalNumOfObjects; //how many objects in the entire scene 
    private PhotonView photonView;
    
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            return;
        }

        float area = Mathf.Abs(xBounds.y - xBounds.x) * Mathf.Abs(zBounds.y - zBounds.x);
        totalNumOfObjects = objDensity * area;

        for (int x = 0; x < totalNumOfObjects; x++)
        {
            GameObject objToSpawn = allObjects[(int)Random.Range(0, allObjects.Count)];
            objectsInScene.Add(objToSpawn);

            objToSpawn.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Vector3 spawnPos = new Vector3(Random.Range(xBounds.x, xBounds.y), yHeight, Random.Range(zBounds.x, zBounds.y));
            PhotonNetwork.Instantiate(objToSpawn.name, spawnPos, Quaternion.identity);
        }
    }

    public void SpawnRandomObject()
    {
        GameObject objToSpawn = allObjects[(int)Random.Range(0, allObjects.Count)];
        objectsInScene.Add(objToSpawn);

        objToSpawn.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Vector3 spawnPos = new Vector3(Random.Range(xBounds.x, xBounds.y), yHeight, Random.Range(zBounds.x, zBounds.y));
        PhotonNetwork.Instantiate(objToSpawn.name, spawnPos, Quaternion.identity);
    }
}
