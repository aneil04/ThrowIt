using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Powerup : MonoBehaviour
{
    //player picks up powerup - ok
    //play cool effect - ok
    //disable graphics and collider - ok
    //change player properties (make a stats script) - ok
    //after a certain amount of time or when a condition is met, destroy this gameobject 
    private PlayerStats playerStats;
    public string type;
    public GameObject powerupEffect;
    public PhotonView photonView;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerStats = other.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                Pickup();
            }
        }
    }

    void Pickup()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        PhotonNetwork.Instantiate(powerupEffect.name, this.transform.position, this.transform.rotation);

        if (type.Equals("test"))
        {
            playerStats.StartCoroutine("IncreaseMoveSpeed");
        }

        PhotonNetwork.Destroy(this.photonView);
    }
}
