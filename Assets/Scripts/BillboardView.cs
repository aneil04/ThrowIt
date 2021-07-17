using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BillboardView : MonoBehaviour
{
    private Transform camTransform;
    private PhotonView photonView;

    void Start()
    {
        camTransform = Camera.main.transform;
        photonView = GameObject.FindObjectOfType<PhotonView>();
    }

    void LateUpdate()
    {
        this.transform.LookAt(transform.position + camTransform.rotation * Vector3.forward);
    }
}
