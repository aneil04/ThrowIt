using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class LoadScene : MonoBehaviourPunCallbacks
{
    public string sceneToLoad;

    public void load()
    {
        PhotonNetwork.LoadLevel(sceneToLoad);
    }
}
