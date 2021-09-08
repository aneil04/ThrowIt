using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviourPunCallbacks
{
    public PlayerStats playerStats;
    public Animator playerAnimator;
    public Rigidbody rb;

    public GameObject gameUI;
    public GameObject deathScreenUI;
    private bool isDead;

    void Start()
    {
        gameUI.SetActive(true);
        deathScreenUI.SetActive(false);
    }

    public void damagePlayer(float amount, string playerName)
    {
        float newHealth = this.playerStats.Health - amount;
        this.playerStats.Health = newHealth;

        if (playerStats.Health <= 0 && !isDead)
        {
            isDead = true;
            displayPlayerInfo(playerName);
        }
    }

    public void displayPlayerInfo(string killedBy)
    {
        Debug.Log("Killed by: " + killedBy);
        gameUI.SetActive(false);
        deathScreenUI.SetActive(true);
    }

    private void displayKillInfo(string displayString)
    {
        Debug.Log(displayString);
    }

    public void playerDeath()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
