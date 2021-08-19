using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DamagePlayer : MonoBehaviour
{
    public Mass objMass;
    public Rigidbody rb;
    public ThrowInfo info;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Player") { return; }

        GameObject playerToDamage = col.gameObject;

        Debug.Log("collision triggered");
        if (!info.getIsThrowing() || info.getSender() == playerToDamage.GetComponent<PhotonView>().ViewID) { return; }
        Debug.Log("collision triggered 2");
        PlayerDamage playerDamage = playerToDamage.GetComponent<PlayerDamage>();

        float damageAmount = 20;
        // rb.velocity.magnitude * objMass.getMass() / 3;
    
        playerDamage.damagePlayer(damageAmount);
    }
}
