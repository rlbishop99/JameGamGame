using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float speed;
    public Transform player;
    public List<Transform> waypoints;
    private Transform target;
    private Queue<Transform> prevTargets = new Queue<Transform>();
    private float targetDistance;

    // Awake is called when the script object is initialised
    void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("FullGear");

        foreach(var item in temp) {

            waypoints.Add(item.transform);

        }


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
        if (targetDistance < 0.01f)
        {
            prevTargets.Enqueue(target);
            prevTargets.Dequeue();
            targetDistance = Mathf.Infinity;
        }

        foreach (Transform waypoint in waypoints)
        {
            if (!prevTargets.Contains(waypoint) && waypoint.position.y > -0.001f)
            {
                float waypointDistance = Vector3.Distance(transform.position, new Vector3(waypoint.position.x, transform.position.y, waypoint.position.z));
                if (waypointDistance < targetDistance)
                {
                    targetDistance = waypointDistance;
                    target = waypoint;
                }
            }
        }
        if (Vector3.Distance(transform.position, player.position) <= targetDistance)
        {
            target = player;
            targetDistance = Vector3.Distance(transform.position, player.position);
        }

        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), step);
    }
}
