using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class SetPlayerName : MonoBehaviour, IPunInstantiateMagicCallback
{
    public TextMeshProUGUI playerNameUI;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;

        playerNameUI.text = (string) instantiationData[0];
        this.name = (string) instantiationData[0];
    }

    void Update()
    {
        if (this.name.Contains("(Clone)"))
        {
            this.name = this.name.Substring(0, this.name.IndexOf("(Clone)"));
            playerNameUI.text = this.name;
        }
    }
}
