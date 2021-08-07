using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BillboardView : MonoBehaviour
{
    private Transform camTransform;
    void LateUpdate()
    {
        if (camTransform == null)
        {
            if (Camera.main != null)
            {
                this.camTransform = Camera.main.transform;
            }
            return;
        }
        this.transform.LookAt(this.transform.position + camTransform.rotation * Vector3.forward);
    }
}
