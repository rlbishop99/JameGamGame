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

        }
    }

    public void MoveParabola(Transform start, Transform end)
    {
        float timeSinceStart = Time.time - startTime;
        float normalizedTime = timeSinceStart / duration;

        if (normalizedTime < 1f)
        {
            Vector3 parabolicPosition = ParabolicInterpolation(start.position, end.position, maxHeight, normalizedTime);
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
        return endPoint;

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Gear") {

            other.gameObject.GetComponentInParent<drop>().isLocked = false;
            Destroy(gameObject);

        }
    }
}
