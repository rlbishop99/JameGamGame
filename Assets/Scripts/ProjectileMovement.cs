using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileMovement : MonoBehaviour
{
    public Transform startPoint; // The starting point
    public Transform endPoint;   // The ending point
    public float maxHeight = 3f; // Height of the parabola
    public float duration = 2f;  // Total duration of the movement

    private float startTime;     // Time when the movement begins

    public GameObject enemy;
    public GameObject crank;
    public bool isEnemy = false;

    private void Awake() {
        
        crank = GameObject.FindGameObjectWithTag("Crank");

    }

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
        return endPoint;

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "GearHit" && !isEnemy) {

            other.gameObject.GetComponentInParent<drop>().isLocked = false;
            AddParentToCrank(other.gameObject, "FullGear");
            Destroy(gameObject);

        } else if(other.gameObject.tag == "GearHit" && isEnemy) {

            Instantiate(enemy, other.transform.position, Quaternion.identity);
            Destroy(gameObject);
        
        }
    }

    public void AddParentToCrank(GameObject child, string tag) {

        Transform t = child.transform;

        while(t.parent != null) {

            if(t.parent.tag == tag) {

                t = t.parent.transform;

                crank.GetComponent<Crank>().downGears.Add(t);

                break;

            }

        }

    }
}
