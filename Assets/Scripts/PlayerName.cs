using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class PlayerName : MonoBehaviour
{
    public static PlayerName instance;
    public TMP_InputField nameInput;
    [SerializeField] private string playerName;
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void setName()
    {
        this.playerName = nameInput.text;
    }

    public string getName()
    {
        return this.playerName;
    }
}
