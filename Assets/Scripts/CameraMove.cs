using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject player;
    public float smoothSpeed = 10f;
    public Vector3 offset;

    void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        if (this.transform.parent != null) {
            return;
        }
        
        Vector3 desiredPos = player.transform.position + offset;
        Vector3 smoothPos = Vector3.Lerp(this.transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        this.transform.position = smoothPos;
    }

    public void SetPlayer(GameObject _player)
    {
        this.player = _player;
    }
}
