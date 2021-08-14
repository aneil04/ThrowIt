using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RespawnObjects : MonoBehaviour
{
    public Vector2 xBounds;
    public Vector2 zBounds;
    private GameObject objManagerGameObject;
    private ObjManager objManagerScript;
    private PhotonView photonView;
    void Start()
    {
        photonView = this.GetComponent<PhotonView>();

        objManagerGameObject = GameObject.Find("ObjManager");
        this.objManagerScript = objManagerGameObject.GetComponent<ObjManager>();
    }
    void Update()
    {
        if(!photonView.IsMine) {return;}

        if (this.transform.position.y < -10) {
            // objManagerScript.SpawnRandomObject();
            PhotonNetwork.Destroy(this.GetComponent<PhotonView>());
        }
    }
}
