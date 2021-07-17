using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public PlayerName playerName;

    //connect to the master server
    public void connectToServer()
    {
        playerName.setName();

        Debug.Log("Connecting to server");
        PhotonNetwork.ConnectUsingSettings();
    }

    //once connected to master, join a lobby
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server. Trying to join lobby...");
        PhotonNetwork.JoinLobby();
    }

    //once joined in a lobby, join a random room
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby. Trying to join room...");
        PhotonNetwork.JoinRandomRoom();
    }

    //if the join random room failed, then create a new room and join it
    //TODO: make sure to check that the error was because there were no availble rooms 
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed. Trying to create room...");
        CreateRoom();
    }

    //code for creating a room
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
        Debug.Log("Created room");
    }

    //load the game scene once joined in a room
    //TODO: check num of players in room and close the room if it is full
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room. Loading level...");
        PhotonNetwork.LoadLevel("FirstMap");
    }
}