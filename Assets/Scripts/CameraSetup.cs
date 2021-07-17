using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraSetup : MonoBehaviour
{
    private CameraMove camMove;
    public GameObject cam;
    [SerializeField] private PhotonView pv;

    void Start()
    {
        if (pv.IsMine)
        {
            camMove = cam.GetComponent<CameraMove>();
            camMove.SetPlayer(this.gameObject);
            cam.transform.SetParent(null);
        }
        else
        {
            cam.SetActive(false);
        }
    }
}
