using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop : MonoBehaviour
{
    public float speed;
    public int dropProbability = 25;
    private bool isMoveDown = true;
    public bool isLocked = true;

    public bool isDown = false;
    private float nextActionTime = 2f;
    private float period = 3f;

    // Update is called once per frame
    void Update()
    {
        // if (Time.time > nextActionTime)
        // {
        //     nextActionTime += period;
        //     if (Random.Range(0, 100) < dropProbability)
        //     {
        //         isLocked = false;
        //     }
        // }

        if (!isLocked)
        {
            isDown = true;
            Vector3 vector = new Vector3(0f, speed * Time.deltaTime, 0f);
            vector *= isMoveDown ? -1 : 1;
            this.transform.Translate(vector, Space.World);

            float yPos = this.transform.position.y;
            if (yPos <= -4f || yPos >= 0.2f)
            {
                Vector3 mPosition;
                Quaternion mRotation;
                this.transform.GetPositionAndRotation(out mPosition, out mRotation);
                mPosition.y = yPos <= -4f ? -4f : 0.2f;

                this.transform.SetPositionAndRotation(mPosition, mRotation);

                isMoveDown = !isMoveDown;
                isLocked = true;
            }
        }
    }
}
