using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner1 : MonoBehaviour
{
    public GameObject grassPrefab;
    public int numOfPrefabs;
    public float dist;

    void Start() {
        for(int x = 0; x < numOfPrefabs; x++) {
            Vector3 spawnPos = new Vector3(0, x * dist, 0);
            Instantiate(grassPrefab, spawnPos, Quaternion.identity);
        }
    }
}
