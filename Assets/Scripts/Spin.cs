using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    // Public variables
    public float spinSpeed = 50f;
    public float dropSpeed = -10f;

    // Private variables
    private float nextActionTime = 2f;
    private float dropPeriod = 2f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextActionTime)
        {
            nextActionTime += dropPeriod;
            // TODO: Don't use pure random. Should have a better algo that only allows some (i.e. not all) platforms to drop, and also guarantees some platforms do drop every time
            if (Random.Range(0, 100) < 25)
            {
                Vector3 position = transform.position;
                position.y *= -1;
                transform.SetPositionAndRotation(position, transform.rotation);
            }
        }
        // TODO: Figure out how to do drop/raises better


        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
