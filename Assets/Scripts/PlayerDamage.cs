using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviourPun
{
    public PlayerStats playerStats;
    public const byte TAKE_DAMAGE_CODE = 3;
    public const byte BROADCAST_KILL = 4;
    public Slider slider;
    public PhotonView pv;
    public Animator playerAnimator;
    public Rigidbody rb;

    void Start()
    {
        playerStats.Health = playerStats.maxHealth;
        slider.maxValue = playerStats.Health;
        slider.value = playerStats.Health;
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public float getHealth()
    {
        return playerStats.Health;
    }

    public void setHealth(float newHealth)
    {
        playerStats.Health = newHealth;
        slider.value = playerStats.Health;
    }

    void LateUpdate()
    {
        if (playerAnimator.GetBool("isHit") && this.rb.velocity.magnitude < .001f)
        {
            playerAnimator.SetBool("isHit", false);
        }

        if (slider.value != playerStats.Health) {
            slider.value = playerStats.Health;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Grabable")
        {
            GameObject obj = col.gameObject;
            Rigidbody objRB = obj.GetComponent<Rigidbody>();
            ThrowInfo info = obj.GetComponent<ThrowInfo>();

            if (info.getIsThrowing())
            {
                this.playerAnimator.SetBool("isHit", true);

                //take damage
                float damage = (objRB.velocity.magnitude * obj.GetComponent<Mass>().getMass()) / 3;
                playerStats.Health -= damage;
                slider.value = playerStats.Health;

                //raise event for all players including local client to damage the object
                object[] data = new object[] { this.gameObject.GetPhotonView().ViewID, objRB.velocity.magnitude, obj.GetComponent<Mass>().getMass(), info.getSender() };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(TAKE_DAMAGE_CODE, data, raiseEventOptions, SendOptions.SendReliable);

                if (playerStats.Health <= 0)
                {
                    playerDeath();
                }
            }
        }
    }

    public void playerDeath()
    {
        GameObject.FindGameObjectsWithTag("Persist")[0].GetComponent<PlayerName>().DestroyThis();

        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("DeathScreen");
        PhotonNetwork.Destroy(this.pv);
    }

    private void damagePlayer(int playerToDamage, float objVelocity, float objMass, int senderViewID)
    {
        GameObject player = PhotonView.Find(playerToDamage).gameObject;
        PlayerDamage playerDamage = player.GetComponent<PlayerDamage>();

        float damage = (objVelocity * objMass) / 3;
        float newHealth = playerDamage.getHealth() - damage;

        playerDamage.setHealth(newHealth);

        if (playerDamage.getHealth() <= 0 && senderViewID == this.pv.ViewID)
        {
            displayKillInfo("You have killed " + player.name + "!");
        }
    }

    private void displayKillInfo(string displayString)
    {
        Debug.Log(displayString);
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code; //get event code

        if (eventCode == TAKE_DAMAGE_CODE) //take damage 
        {
            object[] data = (object[])photonEvent.CustomData;
            damagePlayer((int)data[0], (float)data[1], (float)data[2], (int)data[3]);
        }
    }
}
