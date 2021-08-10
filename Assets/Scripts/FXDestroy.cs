using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class FXDestroy : MonoBehaviour
{
    public PhotonView photonView;

    void Start() {
        StartCoroutine("Destroy");
    }

    IEnumerator Destroy() {
        yield return new WaitForSeconds(3);
        PhotonNetwork.Destroy(this.photonView);
    }
}
