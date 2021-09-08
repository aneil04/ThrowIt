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

    private bool isUsingPowerup;

    void Start()
    {
        // if (!photonView.IsMine) { return; }

        healthSlider.maxValue = maxHealth;
        strengthSlider.maxValue = maxStrength;

        Health = initialHealth;
        Strength = initialStrength;
    }

    #region PlayerFeilds

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

    #endregion

    #region  PlayerGameStats

    private float timeAlive = 0;
    private bool addTime = true;

    private float maxPlayerStrength = 0;

    void Update()
    {
        //time alive
        if (addTime)
        {
            timeAlive += Time.deltaTime;
        }

        //max strength
        if (this.Strength > maxPlayerStrength)
        {
            maxPlayerStrength = this.Strength;
        }
    }

    public void setAddTimeBool(bool value)
    {
        addTime = value;
    }

    //highest leaderboard position 
    public int getMaxRank()
    {
        return GetComponent<Leaderboard>().getMaxRank();
    }

    //number of objects thrown 
    public int getNumOfObjectsThrown()
    {
        return GetComponent<Grab>().getNumOfObjectsThrown();
    }

    //person who killed you
    //kills

    #endregion

    #region  PowerupStuff

    public float powerupTime;
    public float strengthPowerupIncrease;
    public float moveSpeedPowerupIncrease;
    public float jumpForcePowerupIncrease;
    public float healthRegenInterval;
    public float healthRegenAmount;

    private Dictionary<string, int> numOfPowerups = new Dictionary<string, int>();

    IEnumerator IncreaseStrength()
    {
        if (isUsingPowerup)
        {
            yield return null;
        }
        isUsingPowerup = true;

        this.strength += strengthPowerupIncrease;
        yield return new WaitForSeconds(powerupTime);
        this.strength -= strengthPowerupIncrease;

        isUsingPowerup = false;
    }

    IEnumerator IncreaseMoveSpeed()
    {
        if (isUsingPowerup)
        {
            yield return null;
        }
        isUsingPowerup = true;

        this.moveSpeed += moveSpeedPowerupIncrease;
        yield return new WaitForSeconds(powerupTime);
        this.moveSpeed -= moveSpeedPowerupIncrease;

        isUsingPowerup = false;
    }

    IEnumerator HealPlayer()
    {
        if (isUsingPowerup)
        {
            yield return null;
        }
        isUsingPowerup = true;

        while (this.health <= maxHealth)
        {
            this.health += healthRegenAmount;
            if (this.health >= maxHealth)
            {
                this.health = maxHealth;
            }
            yield return new WaitForSeconds(healthRegenInterval);
        }

        isUsingPowerup = false;
    }

    IEnumerator SpawnSheild()
    {
        if (isUsingPowerup)
        {
            yield return null;
        }
        isUsingPowerup = true;

        //spawn sheild
        yield return new WaitForSeconds(powerupTime);
        //despawn shield

        isUsingPowerup = false;
    }

    IEnumerator JumpBoost()
    {
        if (isUsingPowerup)
        {
            yield return null;
        }
        isUsingPowerup = true;

        this.jumpForce += jumpForcePowerupIncrease;
        yield return new WaitForSeconds(powerupTime);
        this.jumpForce -= jumpForcePowerupIncrease;

        isUsingPowerup = false;
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

    #endregion
}
