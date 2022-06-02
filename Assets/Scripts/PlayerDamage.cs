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

    private int lastPlayerDamage = -1;

    public GameObject graphics;
    public GameObject screenUI;
    public AgentManager manager;
    public PlayerMove playerMove;

    void Start()
    {
        gameUI.SetActive(true);
        deathScreenUI.SetActive(false);
    }

    void Update()
    {
        // damagePlayer(1f, this.photonView.ViewID);
        if (transform.position.y <= -10 && !isDead)
        {
            killPlayer(lastPlayerDamage);
        }
    }

    public void damagePlayer(float amount, int killedByViewId)
    {
        float newHealth = this.playerStats.Health - amount;
        this.playerStats.Health = newHealth;
        lastPlayerDamage = killedByViewId;

        if (playerStats.Health <= 0 && !isDead)
        {
            killPlayer(killedByViewId);
        }
    }

    private void killPlayer(int killedByViewId)
    {
        isDead = true;
        if (lastPlayerDamage != -1)
        {
            if (PhotonView.Find(lastPlayerDamage) != null)
            {
                PhotonView.Find(lastPlayerDamage).gameObject.GetComponent<PlayerStats>().incrementKills();
            }
            displayPlayerInfo("---");
        }
        else
        {
            displayPlayerInfo(PhotonView.Find(killedByViewId).Owner.NickName);
        }

        if (manager)
        {
            manager.enabled = false;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnPlayers>().agentDied();
        }
        else if (playerMove)
        {
            playerMove.enabled = false;
        }

        //disable scripts and graphics 
        graphics.SetActive(false);
        screenUI.SetActive(false);

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;

        GetComponent<BoxCollider>().enabled = false;
    }

    public void displayPlayerInfo(string killedBy)
    {
        playerStats.setKilledBy(killedBy);
        playerStats.setFields();
        gameUI.SetActive(false);
        deathScreenUI.SetActive(true);

        if (playerMove && photonView.IsMine)
        {
            GetComponent<IntersitialAd>().LoadAd();
        }
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

    public void revivePlayer()
    {
        Debug.Log("revived player");
    }
}
