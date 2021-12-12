using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    public GameObject agent1;
    public GameObject agent2;

    public Vector2 xBounds;
    public Vector2 zBounds;

    public float moveTimeInterval;
    float moveTimeCurrent = 0;

    private Vector3 velocity = Vector3.zero;
    Vector3 agent1Final = Vector3.zero;
    public float smoothSpeed;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVel;

    void Update()
    {
        moveTimeCurrent -= Time.deltaTime;
        if (moveTimeCurrent <= 0)
        {
            moveTimeCurrent = moveTimeInterval;
            findNewPos();
        }

        moveAgent(agent1Final);
    }

    void findNewPos()
    {
        LayerMask mask = LayerMask.GetMask("Water");

        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(Random.Range(xBounds.x, xBounds.y), 10, Random.Range(zBounds.x, zBounds.y));
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 1000f))
        {
            agent1Final = new Vector3(rayOrigin.x, 11 - hit.distance, rayOrigin.z);
        }
    }

    void moveAgent(Vector3 finalPos)
    {
        if (finalPos == null)
        {
            return;
        }

        float targetAngle = Mathf.Atan2(finalPos.x, finalPos.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(agent1.transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);

        agent1.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        // agent1.GetComponent<Rigidbody>().MovePosition(agent1.transform.position + (agent1.transform.position - finalPos).normalized * Time.deltaTime * 5);
        agent1.transform.position = Vector3.SmoothDamp(agent1.transform.position, finalPos, ref velocity, smoothSpeed);
    }
}