using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ObjSpawner : MonoBehaviour
{
    public float interval;
    public float currentTime;
    public GameObject obj;
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= interval) {
            currentTime = 0;
            PhotonNetwork.Instantiate(obj.name, this.transform.position, this.transform.rotation, 0);
        }
    }
}
