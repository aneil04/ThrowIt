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
    [SerializeField] private float health;
    public float maxHealth;
    public Slider slider;

    void Start()
    {
        slider.maxValue = maxHealth;
        setHealth(maxHealth);
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

    public float setHealth(float newHealth)
    {
        this.health = newHealth;
        slider.value = this.health;

        return this.health;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "floor")
        {
            GameObject obj = col.gameObject;
            Rigidbody objRB = obj.GetComponent<Rigidbody>();

            if (objRB.velocity.magnitude > 1)
            {
                //raise event for all players including local client to damage the object
                object[] data = new object[] { this.gameObject.GetPhotonView().ViewID, objRB.velocity.magnitude };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(TAKE_DAMAGE_CODE, data, raiseEventOptions, SendOptions.SendReliable);
            }
        }
    }

    private void damagePlayer(int viewID, float velMag)
    {
        GameObject player = PhotonView.Find(viewID).gameObject;
        PlayerDamage playerDamage = player.GetComponent<PlayerDamage>();

        float damage = (velMag * 1) / 3; //TODO: change the 1 to be the mass of the object and change the 2 to be a constant 

        float playerCurrentHealth = playerDamage.getHealth();
        float newHealth = playerCurrentHealth - damage;

        playerDamage.setHealth(newHealth);
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code; //get event code

        if (eventCode == TAKE_DAMAGE_CODE) //take damage 
        {
            object[] data = (object[])photonEvent.CustomData;
            damagePlayer((int)data[0], (float)data[1]);
        }
    }
}
