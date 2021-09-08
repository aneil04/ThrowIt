using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinimapColor : MonoBehaviourPun
{
    public Color friendly;
    public Color enemy;
    private Color outlineColor;

    void Start()
    {
        outlineColor = base.photonView.IsMine ? friendly : enemy;
        GetComponent<Renderer>().material.color = outlineColor;
    }
}
