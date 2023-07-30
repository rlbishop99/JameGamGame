using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public Transform player;
    public List<Transform> waypoints;
    private Transform target;
    private Queue<Transform> prevTargets = new Queue<Transform>();
    private float targetDistance;
    private float speed = 1f;
    private NavMeshAgent navMeshAgent;

    // Awake is called when the script object is initialised
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        prevTargets.Enqueue(null);
        prevTargets.Enqueue(null);

        target = waypoints[0];
        targetDistance = Vector3.Distance(transform.position, target.position);
        foreach (Transform waypoint in waypoints)
        {
            float waypointDistance = Vector3.Distance(transform.position, waypoint.position);
            if (waypointDistance < targetDistance)
            {
                targetDistance = waypointDistance;
                target = waypoint;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (GetFlatDistance(transform, player) <= GetFlatDistance(transform, target))
        // {
        //     target = player;
        //     targetDistance = GetFlatDistance(transform, player);
        // }
        if (targetDistance < 0.01f)
        {
            prevTargets.Enqueue(target);
            prevTargets.Dequeue();
            targetDistance = Mathf.Infinity;
        }

        foreach (Transform waypoint in waypoints)
        {
            if (!prevTargets.Contains(waypoint))
            {
                float waypointDistance = Vector3.Distance(transform.position, new Vector3(waypoint.position.x, transform.position.y, waypoint.position.z));
                if (waypointDistance < targetDistance)
                {
                    targetDistance = waypointDistance;
                    target = waypoint;
                }
            }
        }

        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), step);

        // navMeshAgent.destination = target.position;
    }
}
