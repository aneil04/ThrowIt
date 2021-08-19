using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDamage : MonoBehaviourPun
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

    // void LateUpdate()
    // {
    //     if (!photonView.IsMine) { return; }
    // }

    public void displayPlayerInfo()
    {
        Debug.Log("Time alive: " + Time.timeSinceLevelLoad + " seconds");
        gameUI.SetActive(false);
        deathScreenUI.SetActive(true);
    }

    private void displayKillInfo(string displayString)
    {
        Debug.Log(displayString);
    }

    public void playerDeath()
    {
        // GameObject.FindGameObjectsWithTag("Persist")[0].GetComponent<PlayerName>().DestroyThis();

        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("DeathScreen");
        PhotonNetwork.Destroy(this.photonView);
    }

    public void damagePlayer(float amount)
    {
        // if (!base.photonView.IsMine) { return; }
        float newHealth = this.playerStats.Health - amount;
        this.playerStats.Health = newHealth;

        if (playerStats.Health <= 0 && !isDead)
        {
            isDead = true;
            displayPlayerInfo();
        }
    }
}
