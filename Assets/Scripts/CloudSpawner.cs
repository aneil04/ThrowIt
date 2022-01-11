using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] clouds;
    public GameObject[] cloudSpawners;

    private float time = 0;
    public float spawnInterval;
    public float cloudSpeed;

    void Update()
    {
        time += Time.deltaTime;
        if (time > spawnInterval) {
            spawnCloud();
            time = 0;
        }
    }

    private void spawnCloud() {
        int cloudIndex = Random.Range(0, clouds.Length);
        int spawnerIndex = Random.Range(0, cloudSpawners.Length);

        GameObject cloudPrefab = PhotonNetwork.Instantiate(clouds[cloudIndex].name, cloudSpawners[spawnerIndex].transform.position, cloudSpawners[spawnerIndex].transform.rotation);

        cloudPrefab.GetComponent<Rigidbody>().velocity = new Vector3(cloudSpeed, 0, 0);
    }
}
