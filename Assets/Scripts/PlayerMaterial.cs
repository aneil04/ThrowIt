using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMaterial : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    private Renderer playerRenderer;
    // public Color friendly;
    // public Color enemy;

    public List<Color> colors;
    public Color objColor;

    void Start() 
    {
        playerRenderer = gameObject.GetComponent<Renderer>();

        //TODO: put this stuff in another script and use it to control the outline of the player
        // if (photonView.IsMine)
        // {
        //     playerRenderer.material.color = friendly;
        //     return;
        // }

        // playerRenderer.material.color = enemy;

        int colorIndex = (int) Random.Range(0, colors.Count);
        objColor = colors[colorIndex];

        playerRenderer.material.color = objColor;        
    }
}
