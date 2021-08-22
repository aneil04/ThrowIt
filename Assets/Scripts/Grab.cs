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
    public GameObject grabPosObj;

    public GameObject currentGrab;
    private float strength = 10f;
    public float maxStrength;

    [SerializeField] private bool isGrabbing = false;
    public float grabCooldown = 1; //implemented so players don't spam grab and throw 
    private float grabCdTimer = 0;

    public const byte GRAB_OBJ_CODE = 1;
    public const byte THROW_OBJ_CODE = 2;

    public Animator playerAnimator;
    public Animator grabPosAnimator;

    public Transform aimAssistTarget; //make this private with get; and set;

    public GameObject grabBtn;
    public GameObject throwbtn;

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
        if (col.gameObject.tag == "Grabable" && currentCollisions.Contains(col.gameObject))
        {
            Outline outlineScript = col.gameObject.GetComponent<Outline>();
            outlineScript.enabled = false;

            this.currentCollisions.Remove(col.gameObject); //change to store viewID not actual gameobject 
        }
    }

    private bool checkGrabConditions(GameObject obj)
    {
        if (obj.tag != "Grabable") { return false; }
        if (obj.GetComponent<Mass>().getMass() > playerStats.Strength) { return false; }

        //increse strength here

        return true;
    }

    void grabObject(int objViewID, int playerViewID)
    {
        GameObject obj = PhotonView.Find(objViewID).gameObject;
        obj.GetComponent<ObjOwner>().OwnerViewID = playerViewID;
    }

    void throwObject(int objViewID)
    {
        GameObject obj = PhotonView.Find(objViewID).gameObject;
        obj.GetComponent<ObjOwner>().OwnerViewID = -1;
    }

    public void GrabOrThrowObject()
    {
        if (!base.photonView.IsMine) { return; }

        if (!isGrabbing && grabCdTimer <= 0) //grab the object
        {
            if (currentCollisions.Count > 0 && checkGrabConditions(this.currentCollisions[0]))
            {
                grabCdTimer = grabCooldown;
                isGrabbing = true;
                this.currentGrab = this.currentCollisions[0];

                this.currentGrab.GetComponent<ThrowInfo>().setSender(base.photonView.ViewID);

                object[] content = new object[] { this.currentGrab.GetComponent<PhotonView>().ViewID, base.photonView.ViewID };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(GRAB_OBJ_CODE, content, raiseEventOptions, SendOptions.SendReliable);

                this.playerAnimator.SetBool("isCarrying", true);
                this.playerAnimator.SetBool("throw", false);
                this.grabPosAnimator.SetBool("throw", false);
            }
        }
        else if (isGrabbing) //throw the object 
        {
            isGrabbing = false;

            object[] content = new object[] { this.currentGrab.GetComponent<PhotonView>().ViewID };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(THROW_OBJ_CODE, content, raiseEventOptions, SendOptions.SendReliable);

            this.currentGrab = null;

            this.playerAnimator.SetBool("isCarrying", false);
            this.playerAnimator.SetBool("throw", true);
            this.grabPosAnimator.SetBool("throw", true);
        }
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) { return; }

        if (grabCdTimer < 0) { grabCdTimer = 0; } else { grabCdTimer -= Time.fixedDeltaTime; }

        if (playerAnimator.GetBool("throw"))
        {
            this.playerAnimator.SetBool("throw", false);
            this.grabPosAnimator.SetBool("throw", false);
        }

        if (isGrabbing)
        {
            grabBtn.SetActive(false);
            throwbtn.SetActive(true);
        }
        else
        {
            grabBtn.SetActive(true);
            throwbtn.SetActive(false);
        }
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
            throwObject((int)data[0]);
        }
    }
}