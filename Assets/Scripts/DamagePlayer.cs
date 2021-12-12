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
        playerDamage.damagePlayer(damageAmount, info.getSender());
        info.setIsThrowing(false);

        Rigidbody playerRB = playerToDamage.GetComponent<Rigidbody>();
        Vector3 forceVector = this.transform.position - playerToDamage.transform.position;
        forceVector.y = 0;
        playerRB.AddForce(forceVector * 800, ForceMode.Impulse);
    }
}
