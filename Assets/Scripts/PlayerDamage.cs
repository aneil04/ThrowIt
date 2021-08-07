using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviourPun
{
    public const byte TAKE_DAMAGE_CODE = 3;
    public const byte BROADCAST_KILL = 4;
    [SerializeField] private float health;
    public float maxHealth;
    public Slider slider;
    public PhotonView pv;
    public Animator playerAnimator;
    public Rigidbody rb;

    void Start()
    {
        this.health = maxHealth;
        slider.maxValue = health;
        slider.value = this.health;
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
        return this.health;
    }

    public void setHealth(float newHealth)
    {
        this.health = newHealth;
        slider.value = this.health;
    }

    void LateUpdate()
    {
        if (playerAnimator.GetBool("isHit") && this.rb.velocity.magnitude < .001f)
        {
            playerAnimator.SetBool("isHit", false);
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
                this.health -= damage;
                slider.value = this.health;

                //raise event for all players including local client to damage the object
                object[] data = new object[] { this.gameObject.GetPhotonView().ViewID, objRB.velocity.magnitude, obj.GetComponent<Mass>().getMass(), info.getSender() };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(TAKE_DAMAGE_CODE, data, raiseEventOptions, SendOptions.SendReliable);

                if (this.health <= 0)
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
