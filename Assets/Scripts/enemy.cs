using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 targetPosition;

    // Awake is called when the script object is initialised
    void Awake()
    {
        targetPosition = new Vector3(Random.Range(-10f, 10f), transform.position.y, Random.Range(-10f, 10f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 100) < 30)
        {
            targetPosition.x = Random.Range(-10f, 10f);
            targetPosition.z = Random.Range(-10f, 10f);
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
}
