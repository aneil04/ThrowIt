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

    private int lastPlayerDamage = 0;

    void Start()
    {
        gameUI.SetActive(true);
        deathScreenUI.SetActive(false);
    }

    void Update() {
        // damagePlayer(1f, this.photonView.ViewID);
        if (transform.position.y <= -10) {
            damagePlayer(1000, lastPlayerDamage);
            Debug.Log("fell");
        }
    }

    public void damagePlayer(float amount, int killedByViewId)
    {
        float newHealth = this.playerStats.Health - amount;
        this.playerStats.Health = newHealth;
        lastPlayerDamage = killedByViewId;

        if (playerStats.Health <= 0 && !isDead)
        {
            isDead = true;
            PhotonView.Find(killedByViewId).gameObject.GetComponent<PlayerStats>().incrementKills();
            displayPlayerInfo(PhotonView.Find(killedByViewId).Owner.NickName);
        }
    }

    public void displayPlayerInfo(string killedBy)
    {
        playerStats.setKilledBy(killedBy);
        playerStats.setFields();
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

    public void revivePlayer() {
        Debug.Log("revived player");
    }
}
