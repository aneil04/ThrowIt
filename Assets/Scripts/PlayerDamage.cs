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

    void Update() {
        // damagePlayer(1f, this.photonView.ViewID);
    }

    public void damagePlayer(float amount, int killedByViewId)
    {
        float newHealth = this.playerStats.Health - amount;
        this.playerStats.Health = newHealth;

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
