﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Grab : MonoBehaviourPun
{
    List<GameObject> currentCollisions = new List<GameObject>();
    // public InputAction grabInput;
    public GameObject grabPosObj;
    [SerializeField] private bool isGrabbing = false;
    public GameObject currentGrab;
    public float strength;
    public float grabCooldown = 1; //implemented so players don't spam grab and throw 
    private float grabCdTimer = 0;
    public const byte GRAB_OBJ_CODE = 1;
    public const byte THROW_OBJ_CODE = 2;
    public Animator playerAnimator;
    private PhotonView myPhotonView;
    private float grabInputVal = 0;
    private void Start()
    {
        myPhotonView = GetComponentInParent<PhotonView>();
    }

    private void OnEnable()
    {
        // grabInput.Enable();
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        // grabInput.Disable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    private void OnTriggerEnter(Collider col)
    {
        this.currentCollisions.Add(col.gameObject);
    }

    private void OnTriggerExit(Collider col)
    {
        this.currentCollisions.Remove(col.gameObject);
    }

    //calls the grabObject method for all clients 
    private void callGrab()
    {
        if (this.currentCollisions[0].gameObject.tag != "Grabable") { return; }

        //assign variables 
        this.grabCdTimer = grabCooldown; //reset timer
        this.isGrabbing = true;
        this.currentGrab = this.currentCollisions[0].gameObject; //set the object grabbed to currentGrab

        this.playerAnimator.SetBool("isCarry", true);

        //raise event for all players including local client to grab the object
        object[] data = new object[] { this.currentGrab.GetPhotonView().ViewID, this.myPhotonView.ViewID }; //object array of data to send 

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(GRAB_OBJ_CODE, data, raiseEventOptions, SendOptions.SendReliable);
    }

    //calls the throwObject method for all clients 
    private void callThrow()
    {
        //remove parenting from grabbed object
        this.currentGrab.transform.SetParent(null);
        this.currentCollisions.Remove(this.currentGrab);

        this.playerAnimator.SetBool("isCarry", false);

        //raise event for all players including local client to throw the object
        object[] data = new object[] { this.currentGrab.GetPhotonView().ViewID, this.myPhotonView.ViewID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(THROW_OBJ_CODE, data, raiseEventOptions, SendOptions.SendReliable);

        //reset variables
        isGrabbing = false;
        currentGrab = null;
        grabCdTimer = grabCooldown;
    }

    public void updateGrabInput() {
        grabInputVal = 1;
    }

    void FixedUpdate()
    {
        if (!myPhotonView.IsMine) { return; }

        grabCdTimer -= Time.fixedDeltaTime;
        if (grabCdTimer < 0) { grabCdTimer = 0; }

        if (grabInputVal == 1 && isGrabbing == false && grabCdTimer == 0)
        {
            if (currentCollisions.Count > 0)
            {
                callGrab();
            }
        }

        if (grabInputVal == 1 && isGrabbing && grabCdTimer == 0)
        {
            callThrow();
        }

        grabInputVal = 0;
    }

    private void grabObject(int grabObjViewID, int otherPlayerPV)
    {
        //find gameobject of the object that is grabbed and transform of the grab position 
        GameObject obj = PhotonView.Find(grabObjViewID).gameObject;
        Transform grabPos = PhotonView.Find(otherPlayerPV).gameObject.transform.Find("Grab Pos");

        //get rigidbody, collider, and photon transform view
        Rigidbody grabRigidbody = obj.GetComponent<Rigidbody>();
        BoxCollider grabCollider = obj.GetComponent<BoxCollider>();
        PhotonTransformView objPTV = obj.GetComponent<PhotonTransformView>();

        //disable rigidbody, collider, and photon transform view
        grabRigidbody.isKinematic = true;
        grabCollider.enabled = false;
        objPTV.enabled = false; //since the object is a child of the player, it doesn't need its own PTV

        //set parenting
        obj.transform.SetParent(grabPos);

        //reset position of gameobject to grab position 
        obj.transform.position = grabPos.position;
        obj.transform.rotation = grabPos.rotation;
    }

    //TODO: make sure to throw the object with the specific players strenght
    private void throwObject(int grabObjViewID, int otherPlayerPV)
    {
        //find gameobject of the object that is grabbed and transform of the grab position 
        GameObject obj = PhotonView.Find(grabObjViewID).gameObject;
        Transform grabPos = PhotonView.Find(otherPlayerPV).gameObject.transform.Find("Grab Pos");

        //get rigidbody, collider, and photon transform view
        Rigidbody grabRigidbody = obj.GetComponent<Rigidbody>();
        BoxCollider grabCollider = obj.GetComponent<BoxCollider>();
        PhotonTransformView objPTV = obj.GetComponent<PhotonTransformView>();

        //clear parenting
        obj.transform.SetParent(null);

        //enable rigidbody, collider, and photon transform view
        grabRigidbody.isKinematic = false;
        grabCollider.enabled = true;
        objPTV.enabled = true;

        //throw the object 
        grabRigidbody.AddForce(grabPos.transform.forward * strength);
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code; //get event code

        if (eventCode == GRAB_OBJ_CODE) //grab the object   
        {
            object[] data = (object[])photonEvent.CustomData;
            grabObject((int)data[0], (int)data[1]);
        }
        else if (eventCode == THROW_OBJ_CODE) //throw the object 
        {
            object[] data = (object[])photonEvent.CustomData;
            throwObject((int)data[0], (int)data[1]);
        }
    }
}