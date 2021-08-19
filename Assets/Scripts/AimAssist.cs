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
    public PhotonView photonView;

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (other.gameObject.GetComponent<PhotonView>().ViewID != photonView.ViewID)
        {
            targets.Add(other.gameObject.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        targets.Remove(other.gameObject.transform);
    }

    void LateUpdate()
    {
        foreach (Transform target in targets)
        {
            if (target != null)
            {
                Vector3 targetPosition = target.position - this.transform.position;
                float angle = Vector3.Angle(targetPosition, transform.forward);

                if (angle <= aimRange)
                {
                    grab.aimAssistTarget = target;
                }
            }
        }
    }
}
