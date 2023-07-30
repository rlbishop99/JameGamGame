using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileMovement : MonoBehaviour
{
    public Transform startPoint; // The starting point
    public Transform endPoint;   // The ending point
    public float maxHeight = 3f; // Height of the parabola
    public float duration = 2f;  // Total duration of the movement

    private float startTime;     // Time when the movement begins

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if(startPoint != null && endPoint != null) {

            MoveParabola(startPoint, endPoint);
            //Debug.Log("Startpoint: " + startPoint.position + "Endpoint: " + endPoint.position);

        }
    }

    public void MoveParabola(Transform pointA, Transform pointB)
    {
        float timeSinceStart = Time.time - startTime;
        float normalizedTime = timeSinceStart / duration;

        if (normalizedTime < 1f)
        {
            Vector3 parabolicPosition = ParabolicInterpolation(pointA.position, pointB.position, maxHeight, normalizedTime);
            transform.position = parabolicPosition;
        }
        else
        {            
        }
    }

    private Vector3 ParabolicInterpolation(Vector3 start, Vector3 end, float height, float t)
    {
        Vector3 result = (1 - t) * start + t * end;

        // Apply the height to the y-axis
        result.y += height * Mathf.Sin(t * Mathf.PI);

        return result;
    }

    public Transform AssignStart(Transform start) {

        startPoint = start;
        return startPoint;

    }

    public Transform AssignEnd(Transform end) {

        endPoint = end;
        Debug.Log(end.position);
        Debug.Log(endPoint.position);
        return endPoint;

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "GearHit") {

            other.gameObject.GetComponentInParent<drop>().isLocked = false;
            Destroy(gameObject);

        }
    }
}
