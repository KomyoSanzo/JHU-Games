using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Seeker))]
public class AIFlyScript : SimpleCharacterController {

    //What to chase
    public Transform target;

    //How many times per second we update
    public float updateRate = 2f;

    private Seeker seeker;

    public Path path;

    [HideInInspector]
    public bool pathIsEnded = false;
    
    //The distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;

	// Use this for initialization
	void Start () {
        base.Start();
        seeker = GetComponent<Seeker>();
        if(target == null)
        {
            Debug.LogError("No Player found for enemy to chase");
            return;
        }

        //Start a new path to a target position and return result to onPathComplete Function
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }
	
    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path! Did it have an error? " + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator UpdatePath()
    {
        if(target == null)
        {
            Debug.LogError("Target not found?!?!?!");
            //TODO: Insert a player search here. 
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

	// Update is called once per frame
	void FixedUpdate () {
	    if(target== null)
        {
            Debug.LogError("Cant find target!");
            return;
        }

        //TODO: Always Look at player

        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }

            Debug.Log("End of Path is Reached");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        //Direction of enemy
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= moveSpeed * Time.fixedDeltaTime;

        controller.Move(dir);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if(dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }

    }
}
