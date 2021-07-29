using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DisableUI : MonoBehaviour
{
    public PhotonView photonView;
    void Start()
    {
        if (!photonView.IsMine) {
            this.gameObject.SetActive(false);
        }    
    }
}
