using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float speed;

    public bool isLeft = false;
    
    // Update is called once per frame
    void Update()
    {
        if(!isLeft) {
            
            this.transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.World);

        } else {

            this.transform.Rotate(Vector3.down, speed * Time.deltaTime, Space.World);


        }
    }
}
