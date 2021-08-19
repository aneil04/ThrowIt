using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour, IPunObservable
{
    public PhotonView photonView;

    public Slider healthSlider;
    public Slider strengthSlider;

    public float maxHealth;
    public float maxStrength;
    public float initialHealth;
    public float initialStrength;

    private float health = 100f;
    private float strength = 10f;
    private float moveSpeed = 8f;
    private float jumpForce = 100f;

    void Start()
    {
        // if (!photonView.IsMine) { return; }

        healthSlider.maxValue = maxHealth;
        strengthSlider.maxValue = maxStrength;

        Health = initialHealth;
        Strength = initialStrength;
    }

    public float Health
    {
        get { return this.health; }
        set
        {
            this.health = value;
            healthSlider.value = this.health;
        }
    }

    public float Strength
    {
        get { return this.strength; }
        set
        {
            this.strength = value;
            strengthSlider.value = this.strength;
        }
    }

    public float MoveSpeed
    {
        get { return this.moveSpeed; }
        set { this.moveSpeed = value; }
    }

    //powerup stuff here
    public float powerupTime;
    public float strengthPowerupIncrease;
    public float moveSpeedPowerupIncrease;
    public float jumpForcePowerupIncrease;
    public float healthRegenInterval;
    public float healthRegenAmount;

    IEnumerator IncreaseStrength()
    {
        this.strength += strengthPowerupIncrease;
        yield return new WaitForSeconds(powerupTime);
        this.strength -= strengthPowerupIncrease;
    }

    IEnumerator IncreaseMoveSpeed()
    {
        this.moveSpeed += moveSpeedPowerupIncrease;
        yield return new WaitForSeconds(powerupTime);
        this.moveSpeed -= moveSpeedPowerupIncrease;
    }

    IEnumerator HealPlayer()
    {
        while (this.health <= maxHealth)
        {
            this.health += healthRegenAmount;
            if (this.health >= maxHealth)
            {
                this.health = maxHealth;
            }
            yield return new WaitForSeconds(healthRegenInterval);
        }
    }

    IEnumerator SpawnSheild()
    {
        //spawn sheild
        yield return new WaitForSeconds(powerupTime);
        //despawn shield
    }

    IEnumerator JumpBoost()
    {
        this.jumpForce += jumpForcePowerupIncrease;
        yield return new WaitForSeconds(powerupTime);
        this.jumpForce -= jumpForcePowerupIncrease;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //send data
        {
            stream.SendNext(this.Health);
            stream.SendNext(this.Strength);
        }
        else //recieve data
        {
            this.Health = (float)stream.ReceiveNext();
            this.Strength = (float)stream.ReceiveNext();
        }
    }
}
