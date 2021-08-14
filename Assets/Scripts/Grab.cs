using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class Grab : MonoBehaviourPun
{
    public PlayerStats playerStats;
    List<GameObject> currentCollisions = new List<GameObject>();
    // public InputAction grabInput;
    public GameObject grabPosObj;
    [SerializeField] private bool isGrabbing = false;
    public GameObject currentGrab;
    private float strength = 10f;
    public Slider slider;
    public float maxStrength;
    public float grabCooldown = 1; //implemented so players don't spam grab and throw 
    private float grabCdTimer = 0;
    public const byte GRAB_OBJ_CODE = 1;
    public const byte THROW_OBJ_CODE = 2;
    public Animator playerAnimator;
    public Animator grabPosAnimator;
    private PhotonView myPhotonView;
    private float grabInputVal = 0;
    private bool isThrowing = false;
    public float throwDelay;
    private float throwDelayTime = 0f;
    public Transform aimAssistTarget; //make this private with get; and set;

    private void Start()
    {
        myPhotonView = GetComponentInParent<PhotonView>();
        slider.maxValue = maxStrength;
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Grabable" && col.gameObject.GetComponent<Mass>().getMass() < playerStats.Strength)
        {
            Outline outlineScript = col.gameObject.GetComponent<Outline>();
            outlineScript.enabled = true;

            this.currentCollisions.Add(col.gameObject);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Grabable" && col.gameObject.GetComponent<Mass>().getMass() < playerStats.Strength)
        {
            Outline outlineScript = col.gameObject.GetComponent<Outline>();
            outlineScript.enabled = false;

            this.currentCollisions.Remove(col.gameObject);
        }
    }

    //calls the grabObject method for all clients 
    private void callGrab()
    {
        if (this.currentCollisions[0].gameObject.tag != "Grabable") { return; }

        if (this.currentCollisions[0].gameObject.GetComponent<Mass>().getMass() > playerStats.Strength) { return; }

        //assign variables 
        this.grabCdTimer = grabCooldown; //reset timer
        this.isGrabbing = true;
        this.currentGrab = this.currentCollisions[0].gameObject; //set the object grabbed to currentGrab

        playerStats.Strength += this.currentGrab.GetComponent<Mass>().getMass() / 2;
        slider.value = playerStats.Strength;

        this.playerAnimator.SetBool("isCarrying", true); //play animation
        this.playerAnimator.SetBool("throw", false);
        this.grabPosAnimator.SetBool("throw", false);

        //raise event for all players including local client to grab the object
        object[] data = new object[] { this.currentGrab.GetPhotonView().ViewID, this.myPhotonView.ViewID }; //object array of data to send 

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(GRAB_OBJ_CODE, data, raiseEventOptions, SendOptions.SendReliable);
    }

    //calls the throwObject method for all clients 
    private void callThrow()
    {
        if (aimAssistTarget != null)
        {
            grabPosObj.transform.LookAt(aimAssistTarget);
        }

        //remove outline
        Outline outlineScript = this.currentGrab.GetComponent<Outline>();
        outlineScript.enabled = false;

        //remove parenting from grabbed object
        this.currentGrab.transform.SetParent(null);
        this.currentCollisions.Remove(this.currentGrab);

        // this.playerAnimator.SetBool("throw", true); //play animation
        // this.playerAnimator.SetBool("isCarrying", false);

        //raise event for all players including local client to throw the object
        object[] data = new object[] { this.currentGrab.GetPhotonView().ViewID, this.myPhotonView.ViewID, grabPosObj.transform.eulerAngles };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(THROW_OBJ_CODE, data, raiseEventOptions, SendOptions.SendReliable);

        //reset variables
        isGrabbing = false;
        currentGrab = null;
        grabCdTimer = grabCooldown;
    }

    public void updateGrabInput()
    {
        grabInputVal = 1;
    }

    void Update()
    {
        aimAssistTarget = null; //maybe change this so its a bit more performance effieict 
    }

    void FixedUpdate()
    {
        if (!myPhotonView.IsMine) { return; }

        this.slider.value = playerStats.Strength;

        if (playerAnimator.GetBool("throw"))
        {
            this.playerAnimator.SetBool("throw", false);
            this.grabPosAnimator.SetBool("throw", false);
        }

        grabCdTimer -= Time.fixedDeltaTime;
        if (grabCdTimer < 0) { grabCdTimer = 0; }

        if (grabInputVal == 1 && isGrabbing == false && grabCdTimer == 0)
        {
            if (currentCollisions.Count > 0)
            {
                callGrab();
            }
        }

        throwDelayTime += Time.fixedDeltaTime;
        if (grabInputVal == 1 && isGrabbing && grabCdTimer == 0 && isThrowing == false)
        {
            isThrowing = true;
            throwDelayTime = 0f;

            this.playerAnimator.SetBool("throw", true); //play animation
            this.playerAnimator.SetBool("isCarrying", false);
            this.grabPosAnimator.SetBool("throw", true);
        }


        if (throwDelayTime >= throwDelay && isThrowing)
        {
            isThrowing = false;
            // this.playerAnimator.SetBool("throw", false);
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

        //set the ThrowInfo field
        ThrowInfo info = obj.GetComponent<ThrowInfo>();
        info.setSender(this.photonView.ViewID);
        info.setIsThrowing(true);

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
    private void throwObject(int grabObjViewID, int otherPlayerPV, Vector3 euler)
    {
        //find gameobject of the object that is grabbed and transform of the grab position 
        GameObject obj = PhotonView.Find(grabObjViewID).gameObject;
        GameObject otherPlayer = PhotonView.Find(otherPlayerPV).gameObject;
        Transform grabPos = otherPlayer.transform.Find("Grab Pos");

        grabPos.rotation = Quaternion.Euler(euler);

        //get rigidbody, collider, and photon transform view
        Rigidbody grabRigidbody = obj.GetComponent<Rigidbody>();
        BoxCollider grabCollider = obj.GetComponent<BoxCollider>();
        PhotonTransformView objPTV = obj.GetComponent<PhotonTransformView>();

        PlayerStats stats = otherPlayer.GetComponent<PlayerStats>();

        //clear parenting
        obj.transform.SetParent(null);

        //enable rigidbody, collider, and photon transform view
        grabRigidbody.isKinematic = false;
        grabCollider.enabled = true;
        objPTV.enabled = true;

        //throw the object 
        float min = 750;
        float max = 1300;
        float power = (stats.Strength * (max - min) / 250) + min;
        grabRigidbody.AddForce(grabPos.transform.forward * power);
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
            throwObject((int)data[0], (int)data[1], (Vector3)data[2]);
        }
    }
}