using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    // public InputAction input;
    public Joystick joystick;
    public bool inputType = true; //true if keyboard and mouse, false if joystick
    public Rigidbody rb;
    public float speed;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVel;
    [SerializeField] private PhotonView pv;
    public Animator playerAnimator;

    // void OnEnable()
    // {
    //     input.Enable();
    // }

    // void OnDisable()
    // {
    //     input.Disable();
    // }

    void FixedUpdate()
    {
        if (!pv.IsMine) { return; }

        performMovement();
    }

    void performMovement()
    {
        // Vector2 inputVector = new Vector2(0, 0);
        if (inputType)
        {
            // inputVector = input.ReadValue<Vector2>().normalized;
        }
        else
        {
            // inputVector = new Vector2(joystick.Horizontal, joystick.Vertical);
        }

        Vector3 moveDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;

        if (moveDir.magnitude > 0.1f)
        {
            playerAnimator.SetBool("isRun", true);

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            rb.MovePosition(transform.position + moveDir * Time.deltaTime * speed);
        }
        else
        {
            playerAnimator.SetBool("isRun", false);
        }
    }
}
