using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject player;
    public float smoothSpeed = 10f;
    public Vector3 offset;
    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
        {
            Destroy(this.gameObject);
            Debug.Log("destroyed camera");
        }
        
        Vector3 desiredPos = player.transform.position + offset;
        Vector3 smoothPos = Vector3.Lerp(this.transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        this.transform.position = smoothPos;
        this.transform.LookAt(player.transform);
    }

    public void SetPlayer(GameObject _player)
    {
        this.player = _player;
    }
}
