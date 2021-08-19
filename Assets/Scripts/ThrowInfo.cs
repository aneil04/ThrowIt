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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (this.senderPV != -1 && this.rb.velocity.magnitude < 1)
        {
            Debug.Log("got in here");
            this.senderPV = -1;
            isThrowing = false;
        }
    }
}
