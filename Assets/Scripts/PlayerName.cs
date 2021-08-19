using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class PlayerName : MonoBehaviour
{
    public void SetPlayerName(string value) {
        if (value == null || value == "") {
            return;
        }

        PhotonNetwork.NickName = value;
    }
}
