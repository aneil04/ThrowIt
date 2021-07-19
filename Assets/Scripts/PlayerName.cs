using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class PlayerName : MonoBehaviour
{
    public static PlayerName instance;
    public TMP_InputField nameInput;
    private string playerName;

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
    public void FindInput()
    {
        nameInput = GameObject.Find("UI").GetComponentInChildren<Canvas>().GetComponentInChildren<TMP_InputField>();

        setName();
    }
    public void setName()
    {
        string txt = nameInput.text;
        this.playerName = txt;
    }

    public string getName()
    {
        return this.playerName;
    }

    public void DestroyThis() {
        Destroy(this.gameObject);
    }
}
