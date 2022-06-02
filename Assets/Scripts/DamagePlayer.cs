using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class DamagePlayer : MonoBehaviour
{
    private Mass objMass;
    private Rigidbody rb;
    private ThrowInfo info;

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
        if (playerToDamage == null) {
            return;
        }
        
        if (!info.getIsThrowing() || info.getSender() == playerToDamage.GetComponent<PhotonView>().ViewID) { return; }

        PlayerDamage playerDamage = playerToDamage.GetComponent<PlayerDamage>();
        float damageAmount = 20;
        // rb.velocity.magnitude * objMass.getMass() / 3;
        playerDamage.damagePlayer(damageAmount, info.getSender());
        info.setIsThrowing(false);

        Rigidbody playerRB = playerToDamage.GetComponent<Rigidbody>();
        Vector3 forceVector = playerToDamage.transform.position - this.transform.position;
        forceVector.y = 0;
        playerRB.AddForce(forceVector.normalized * 800, ForceMode.Impulse);

        if (playerToDamage.GetComponent<NavMeshAgent>()) { //ai
            playerToDamage.GetComponent<AgentManager>().isHit = true;
        } else { //player
            playerToDamage.GetComponent<PlayerMove>().isHit = true;
        }

        // playerRB.AddExplosionForce(800, col.contacts[0].point, 0.1f, 0, ForceMode.Impulse);
    }
}
