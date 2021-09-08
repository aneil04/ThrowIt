using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ThrowInfo : MonoBehaviourPun
{
    private int senderPV; //the person who threw the object
    private bool isThrowing;

    public int getSender() { return this.senderPV; }

    public void setSender(int senderPV) { this.senderPV = senderPV; }

    public bool getIsThrowing() { return this.isThrowing; }
    public void setIsThrowing(bool value) { this.isThrowing = value; }

    public float throwReset;
    private float throwResetTime;
    private Rigidbody rb;

    private bool hasTrail;
    public GameObject trailPrefab;
    private GameObject trail;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (this.senderPV != -1 && this.rb.velocity.magnitude < 1)
        {
            this.senderPV = -1;
            isThrowing = false;
        }

        if (isThrowing && !hasTrail)
        {
            hasTrail = true;

            trail = Instantiate(trailPrefab, this.transform.position, Quaternion.identity);
            trail.transform.SetParent(this.transform);
        }

        if (!isThrowing && hasTrail)
        {
            hasTrail = false;
            GameObject.Destroy(trail);
        }
    }
}
