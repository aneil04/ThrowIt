using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AimAssist : MonoBehaviour
{
    public Transform grabPos;
    private List<Transform> targets = new List<Transform>();
    public float aimRange;
    public Grab grab;
    [SerializeField] private PhotonView photonView;
    private GameObject target;

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (other.gameObject.GetComponent<PhotonView>().ViewID != this.photonView.ViewID)
        {
            if (target == null) {
                target = other.gameObject;
            } else if (Vector3.Distance(other.gameObject.transform.position, this.transform.position) < Vector3.Distance(target.gameObject.transform.position, this.transform.position)) {
                target = other.gameObject;
                Debug.Log("set target");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (target == other.gameObject) {
            target = null;
        }
        targets.Remove(other.gameObject.transform);
    }

    void LateUpdate()
    {
        if (target != null) {
            this.transform.LookAt(target.transform.position);
        }
        // bool setTarget = false;
        // foreach (Transform target in targets)
        // {
        //     if (target != null)
        //     {
        //         Vector3 targetPosition = target.position - this.transform.position;
        //         float angle = Vector3.Angle(targetPosition, transform.forward);

        //         if (Mathf.Abs(angle) <= aimRange)
        //         {
        //             Debug.Log("set target");
        //             grab.aimAssistTarget = target;
        //             setTarget = true;
        //         }
        //     }
        // }

        // if (!setTarget) {
        //     grab.aimAssistTarget = null;
        // }
    }
}
