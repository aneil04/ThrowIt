using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMaterial : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    public Color[] skinColors;
    public Color[] shirtColors;
    public Color[] legColors;
    private Color skinColor;
    private Color shirtColor;
    private Color legColor;
    public Outline outline;

    Vector3 getVectorFromColor(Color color)
    {
        return new Vector3(color.r, color.g, color.b);
    }

    Color getColorFromVector(Vector3 color)
    {
        return new Color(color.x, color.y, color.z);
    }

    void Start()
    {


        //TODO: put this stuff in another script and use it to control the outline of the player
        // if (photonView.IsMine)
        // {
        //     playerRenderer.material.color = friendly;
        //     return;
        // }

        // playerRenderer.material.color = enemy;

        int colorIndex = 0;

        colorIndex = (int)Random.Range(0, skinColors.Length);
        skinColor = skinColors[colorIndex];

        colorIndex = (int)Random.Range(0, shirtColors.Length);
        shirtColor = shirtColors[colorIndex];

        colorIndex = (int)Random.Range(0, legColors.Length);
        legColor = legColors[colorIndex];

        photonView.RPC("SetPlayerColor", RpcTarget.AllBuffered, getVectorFromColor(skinColor), getVectorFromColor(shirtColor), getVectorFromColor(legColor), this.photonView.ViewID);
    }

    [PunRPC]
    void SetPlayerColor(Vector3 skinColor, Vector3 shirtColor, Vector3 legColor, int viewID)
    {
        GameObject targetPlayer = PhotonView.Find(viewID).gameObject;

        GameObject targetGraphics = targetPlayer.transform.Find("Graphics").gameObject;
        GameObject targetCharacter = targetGraphics.transform.Find("Character").gameObject;

        targetCharacter.transform.Find("Arms and Head").gameObject.GetComponent<Renderer>().material.color = getColorFromVector(skinColor);
        targetCharacter.transform.Find("Shirt").gameObject.GetComponent<Renderer>().material.color = getColorFromVector(shirtColor);
        targetCharacter.transform.Find("Legs").gameObject.GetComponent<Renderer>().material.color = getColorFromVector(legColor);
    }
}
