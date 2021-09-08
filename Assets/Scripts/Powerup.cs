using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Powerup : MonoBehaviour
{
    private PlayerStats playerStats;
    private string type;
    public List<string> powerups = new List<string>();
    public GameObject powerupEffect;
    public PhotonView photonView;

    public Rigidbody rb;
    public BoxCollider boxCollider;
    private bool isActive = false;

    private float elapsedTime = 0;

    ObjManager objManager;

    void Start()
    {
        type = powerups[(int)Random.Range(0, powerups.Count)];
        objManager = GameObject.FindGameObjectWithTag("ObjManager").GetComponent<ObjManager>();
    }

    void Update()
    {
        if (rb.velocity.magnitude <= 0.1f && elapsedTime > 1)
        {
            isActive = true;
            rb.isKinematic = true;
            boxCollider.enabled = false;
        }

        if (!isActive)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive) { return; }

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

        switch (type)
        {
            case "heal":
                playerStats.StartCoroutine("HealPlayer");
                break;
            case "speedBoost":
                playerStats.StartCoroutine("IncreaseMoveSpeed");
                break;
            case "strengthBoost":
                playerStats.StartCoroutine("IncreaseStrength");
                break;
            case "sheild":
                playerStats.StartCoroutine("SpawnSheild");
                break;
            case "jumpBoost":
                playerStats.StartCoroutine("JumpBoost");
                break;
            default:
                break;
        }

        objManager.decrementNumOfPowerup();
        PhotonNetwork.Destroy(this.photonView);
    }
}
