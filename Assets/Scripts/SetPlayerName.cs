using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class SetPlayerName : MonoBehaviour
{
    public TextMeshProUGUI playerNameUI;

    void Update()
    {
        if (playerNameUI.text != (string)PhotonNetwork.NickName)
        {
            playerNameUI.text = (string)PhotonNetwork.NickName;
        }
    }
}
