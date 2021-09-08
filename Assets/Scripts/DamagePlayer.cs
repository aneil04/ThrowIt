using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DamagePlayer : MonoBehaviour
{
    public Mass objMass;
    public Rigidbody rb;
    public ThrowInfo info;

    void Start() {
        objMass = GetComponent<Mass>();
        rb = GetComponent<Rigidbody>();
        info = GetComponent<ThrowInfo>();
    }

    //TODO: unfreeze the position of the player when collided with an object 
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Player") { return; }

        GameObject playerToDamage = col.gameObject;

        if (!info.getIsThrowing() || info.getSender() == playerToDamage.GetComponent<PhotonView>().ViewID) { return; }

        PlayerDamage playerDamage = playerToDamage.GetComponent<PlayerDamage>();
        float damageAmount = 20;
        // rb.velocity.magnitude * objMass.getMass() / 3;
        playerDamage.damagePlayer(damageAmount, PhotonView.Find(info.getSender()).Owner.NickName);

        info.setIsThrowing(false);
    }
}
