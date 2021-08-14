using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    public Transform grabPos;
    private List<Transform> targets = new List<Transform>();
    public float aimRange;
    public Grab grab;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false)
        {
            return;
        }

        targets.Add(other.gameObject.transform);
    }

    void OnTriggerExit(Collider other)
    {
        targets.Remove(other.gameObject.transform);
    }

    void LateUpdate()
    {
        foreach (Transform target in targets)
        {
            Vector3 targetPosition = target.position - this.transform.position;
            float angle = Vector3.Angle(targetPosition, transform.forward);

            if (angle <= aimRange) {
                grab.aimAssistTarget = target;
            }
        }
    }
}
