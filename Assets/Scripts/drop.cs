using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop : MonoBehaviour
{
    public float speed;
    public int dropProbability = 25;
    private bool isMoveDown = true;
    public bool isLocked = true;

    public GameObject crank;

    private void Awake() {
        
        crank = GameObject.FindGameObjectWithTag("Crank");

    }

    // Update is called once per frame
    void Update()
    {

        if (!isLocked)
        {
            Vector3 vector = new Vector3(0f, speed * Time.deltaTime, 0f);
            vector *= isMoveDown ? -1 : 1;
            this.transform.Translate(vector, Space.World);

            float yPos = this.transform.position.y;

            if (yPos <= -4f || yPos >= 0f)
            {
                Vector3 mPosition;
                Quaternion mRotation;
                this.transform.GetPositionAndRotation(out mPosition, out mRotation);
                mPosition.y = yPos <= -4f ? -4f : 0f;

                this.transform.SetPositionAndRotation(mPosition, mRotation);

                isMoveDown = !isMoveDown;
                isLocked = true;

                if(isMoveDown && yPos >= 0f) {

                    crank.GetComponent<Crank>().ClearGears();
                    
                }

            }
        }
    }
}
