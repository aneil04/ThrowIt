using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using ExitGames.Client.Photon;
using TMPro;

public class Leaderboard : MonoBehaviourPun
{
    private List<int> currentPlayers = new List<int>();

    public float updateInterval;
    private float elapsedTime = 0;

    public const byte PLAYER_JOIN_CODE = 3;

    private TextMeshProUGUI rankTxt;

    private int maxRank = -1;
    public int getMaxRank() {return this.maxRank;}

    void Start()
    {
        rankTxt = GetComponent<TextMeshProUGUI>();

        object[] content = new object[] { base.photonView.ViewID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(PLAYER_JOIN_CODE, content, raiseEventOptions, SendOptions.SendReliable);
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void playerJoin(int other)
    {
        currentPlayers.Add(other);
    }

    //TODO: call this method when the player disconnects from the room
    public void playerLeft(int other)
    {
        currentPlayers.Remove(other);
    }

    void Update()
    {
        if (elapsedTime < 0)
        {
            elapsedTime = updateInterval;
            updateLeaderboard();

        }
        else
        {
            elapsedTime -= Time.deltaTime;
        }
    }

    void updateLeaderboard()
    {
        Dictionary<int, float> playerStrength = new Dictionary<int, float>(); //dict w view id and strength
        List<int> leaderboard = new List<int>(); //list with view id

        for (int x = 0; x < currentPlayers.Count; x++)
        {
            playerStrength.Add(currentPlayers[x], PhotonView.Find(currentPlayers[0]).gameObject.GetComponent<PlayerStats>().Strength);
        }

        foreach (KeyValuePair<int, float> val in playerStrength.OrderBy(key => key.Value)) //least to greatest 
        {
            leaderboard.Add(val.Key);
        }

        int rank = leaderboard.IndexOf(base.photonView.ViewID) + 1;
        rankTxt.text = "Rank: #" + rank;

        if (rank != 0 && rank > maxRank) {
            maxRank = rank;
        }
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code; //get event code

        if (eventCode == PLAYER_JOIN_CODE) //grab the object   
        {
            object[] data = (object[])photonEvent.CustomData;
            playerJoin((int)data[0]);
        }

    }
}