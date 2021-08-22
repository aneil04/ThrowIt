using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjOwner : MonoBehaviourPun
{
    private int ownerViewId = -1;
    private Transform ownerGrabPos;
    private float throwDelay = 0.5f;
    private Rigidbody rigidbody;

    public float power = 40;
    private bool isThrowing;

    public int OwnerViewID
    {
        get { return this.ownerViewId; }
        set { this.ownerViewId = value; }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (this.ownerViewId != -1 && this.ownerGrabPos == null) //executed once after you grab something 
        {
            this.ownerGrabPos = PhotonView.Find(this.ownerViewId).gameObject.transform.GetChild(3);
        }

        if (this.ownerGrabPos != null) // always follow the grab position of the owner 
        {
            this.gameObject.transform.position = this.ownerGrabPos.position;
            this.gameObject.transform.rotation = this.ownerGrabPos.rotation;
        }

        if (this.ownerViewId == -1 && this.ownerGrabPos != null && !isThrowing) //executed once when you throw something
        {
            isThrowing = true;
            StartCoroutine("ThrowObject");
        }
    }

    IEnumerator ThrowObject()
    {
        yield return new WaitForSeconds(throwDelay);

        float min = 75;
        float max = 130;
        // float power = 20; //TODO: change 20 to playerStats.strenght

        //30 to 40 is optimal value 

        rigidbody.AddForce(this.ownerGrabPos.transform.forward * power, ForceMode.Impulse);

        this.GetComponent<ThrowInfo>().setIsThrowing(true);
        
        isThrowing = false;
        this.ownerGrabPos = null;
    }
}
