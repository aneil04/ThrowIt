using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class PlayerName : MonoBehaviour
{
    public string playerName = "";

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    
    public void SetPlayerName(string value)
    {
        playerName = value;
    }
}
