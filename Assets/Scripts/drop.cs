using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop : MonoBehaviour
{
    public float speed;
    private bool isMoveDown = true;
    private bool isLocked = true;
    private float nextActionTime = 2f;
    private float period = 2f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            if (Random.Range(0, 100) < 25)
            {
                isLocked = false;
            }
        }

        if (!isLocked)
        {
            Vector3 vector = new Vector3(0f, speed * Time.deltaTime, 0f);
            vector *= isMoveDown ? -1 : 1;
            this.transform.Translate(vector, Space.World);

            float yPos = this.transform.position.y;
            if (yPos <= -1f || yPos >= 0.2f)
            {
                Vector3 mPosition;
                Quaternion mRotation;
                this.transform.GetPositionAndRotation(out mPosition, out mRotation);
                mPosition.y = yPos <= -1f ? -1f : 0.2f;

                this.transform.SetPositionAndRotation(mPosition, mRotation);

                isMoveDown = !isMoveDown;
                isLocked = true;
            }
        }
    }
}
