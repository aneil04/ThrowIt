using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CloudMove : MonoBehaviour
{
    public float delay;
    private float time = 0;
    private PhotonView pv;
    public float cloudSpeed;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        transform.position += transform.forward * cloudSpeed * Time.deltaTime;

        time += Time.deltaTime;
        if (time > delay)
        {
            PhotonNetwork.Destroy(pv);
        }
    }
}
