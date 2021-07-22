﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    public Joystick joystick;
    public Rigidbody rb;
    public float speed;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVel;
    [SerializeField] private PhotonView pv;
    public Animator playerAnimator;
    private bool isGrounded;
    public float jumpForce;
    public Transform raycastOrigin;
    
    void FixedUpdate()
    {
        if (!pv.IsMine) { return; }

        isGrounded = checkIfGrounded();
        if (isGrounded && this.playerAnimator.GetBool("jump")) {
            this.playerAnimator.SetBool("jump", false);
        }

        if (!this.playerAnimator.GetBool("isHit")) {
            performMovement();
        } 
    }

    void performMovement()
    {
        Vector3 moveDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;

        if (moveDir.magnitude > 0.1f)
        {
            playerAnimator.SetBool("isRunning", true);

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            rb.MovePosition(transform.position + moveDir * Time.deltaTime * speed);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
    }

    private bool checkIfGrounded () {
        LayerMask mask = LayerMask.GetMask("floor");
        return Physics.Raycast(raycastOrigin.position, Vector3.down, .3f, mask);
    }

    public void Jump () {
        playerAnimator.SetBool("jump", true);

        Debug.Log("wanted to jump");
        if (this.isGrounded) {
            Debug.Log("jumpd");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
