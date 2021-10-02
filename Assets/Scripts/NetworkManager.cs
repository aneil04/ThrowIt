using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI nameInput;
    private bool isConnecting;

    public void connectToServer()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            // OnConnectedToMaster();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");

        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }

    //TODO: make sure to check that the error was because there were no availble rooms 
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed. Trying to create room...");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;

        PhotonNetwork.CreateRoom(null, roomOptions);
        // CreateRoom();
    }

    // //code for creating a room
    // private void CreateRoom()
    // {
    //     RoomOptions roomOptions = new RoomOptions();
    //     roomOptions.MaxPlayers = 4;
    //     //TODO: make a random string for the room name
    //     PhotonNetwork.CreateRoom("bob", roomOptions, null);
    //     Debug.Log("Created room");
    //     PhotonNetwork.JoinRoom("bob");
    // }

    //load the game scene once joined in a room
    //TODO: check num of players in room and close the room if it is full
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room. Loading level...");

        if (nameInput.text != null && nameInput.text != "")
        {
            PhotonNetwork.NickName = nameInput.text;
        }

        PhotonNetwork.LoadLevel("FloatingMap");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
    }
}