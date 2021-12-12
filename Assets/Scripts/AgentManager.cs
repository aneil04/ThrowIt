using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class AgentManager : MonoBehaviour
{
    public PhotonView pv;
    //states: searching for object, moving to nearest player, attacking 
    [SerializeField] private int state;
    /*
    0: searching for object
    1: moving to nearest player
    2: attacking
    */

    private GameObject targetObj = null;
    private GameObject nearestEnemy;
    public float moveDistance;
    public float escapeRange;
    public float attackRange;
    public float grabDistance;
    public float minObjDistace;

    public NavMeshAgent agent;

    public PlayerStats playerStats;
    public Grab grab;
    ObjManager manager;
    public ScriptAccessor accessor;

    public Animator playerAnimator;
    public float throwTime;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("ObjManager").GetComponent<ObjManager>();
        state = 0;

        // agent.angularSpeed = 360;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) { return; }

        if (agent.velocity.magnitude > 0.1f)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }

        switch (state)
        {
            case 0:
                search();
                break;
            case 1:
                move();
                break;
            case 2:
                attack();
                break;
            default:
                state = 0;
                break;
        }
    }

    private void search() //state 0
    {
        if (grab.getIsGrabbing())
        {
            state = 1;
        }

        // // get distance to nearest enemy and move away if within certain range
        // if (distToNearestEnemy() < escapeRange)
        // {
        //     Vector3 direction = this.transform.position - nearestEnemy.transform.position;
        //     Vector3 newPos = this.transform.position + direction;
        //     agent.SetDestination(newPos);

        //     return;
        // }

        //get target object if no target identified or target has been picked up
        if (targetObj == null || targetObj.GetComponent<ObjOwner>().OwnerViewID != -1)
        {
            targetObj = getTargetObj();
        }

        //move to target object 
        agent.SetDestination(targetObj.transform.position);

        if (Vector3.Distance(targetObj.transform.position, this.transform.position) <= grabDistance)
        {
            targetObj = null;
            //grab object and enable move state
            grab.GrabOrThrowObject();
            distToNearestEnemy();
            state = 1;
        }
    }

    private float distToNearestEnemy() //min dist to a player that is currently grabbing an object 
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float minDist = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            if (player.GetPhotonView().ViewID != this.gameObject.GetPhotonView().ViewID)
            {
                float dist = Vector3.Distance(this.transform.position, player.transform.position);
                if (dist < minDist)
                {
                    nearestEnemy = player;
                    minDist = dist;
                }
            }
        }

        return minDist;
    }

    private GameObject getTargetObj()
    {
        List<GameObject> sceneObj = manager.getSceneGameobjects();

        GameObject target = null;
        float largestMass = -Mathf.Infinity;
        float minDistance = Mathf.Infinity;

        foreach (GameObject obj in sceneObj)
        {
            if (obj != null && obj.GetComponent<ObjOwner>().OwnerViewID == -1) //checks if object is currently grabbed 
            {
                float distToObj = Vector3.Distance(obj.transform.position, this.transform.position);
                if (distToObj > minObjDistace) //checks if object is in range
                {
                    float mass = obj.GetComponent<Mass>().getMass();
                    if (mass > largestMass && mass < playerStats.Strength && distToObj < minDistance && obj.transform.position.y > 0) //checks if object can be grabbed based on mass and strength 
                    {
                        largestMass = mass;
                        minDistance = distToObj;
                        target = obj;
                    }
                }
            }
        }

        return target;
    }

    private void move() //state 1
    {
        //find nearest enemy 
        if (nearestEnemy == null)
        {
            return;
        }

        float dist = Vector3.Distance(this.transform.position, nearestEnemy.transform.position); //sets the nearestEnemy variable 

        // if an enemy comes within a certain range, then attack 
        if (dist < attackRange)
        {
            state = 2;
            return;
        }

        //move agent
        agent.SetDestination(nearestEnemy.transform.position);
    }

    private bool threwObj = false;
    private void attack() //state 2
    {
        //throw object 
        if (!threwObj)
        {
            grab.GrabOrThrowObject();
            threwObj = true;
            StartCoroutine("ExecuteAfterTime", throwTime);
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        state = 0;
        threwObj = false;
    }
}