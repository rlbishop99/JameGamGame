using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Crank : MonoBehaviour
{
    public List<Transform> downGears;

    public GameObject father;
    private bool isLocked = true;
    private float currentRotation = 0f;
    private const float totalRotation = 360f;

    private void Awake()
    {
        downGears.Clear();
    }

    private void Update()
    {
        if (!isLocked)
        {
            float rotationDegrees = (totalRotation / 2f) * Time.deltaTime; // (360 deg / secs) *  (sec/frame) 
            currentRotation += rotationDegrees;
            transform.Rotate(Vector3.up, rotationDegrees, Space.World);

            if (currentRotation >= totalRotation)
            {
                isLocked = true;
                currentRotation = 0;
            }
        }
    }

    public void CrankUp()
    {
        isLocked = false;
        foreach (Transform gear in downGears)
        {
            gear.gameObject.GetComponent<drop>().isLocked = false;
        }
    }

    public void ClearGears()
    {
        foreach (Transform gear in downGears)
        {
            UpGear(gear);
        }
    }

    public void UpGear(Transform gear)
    {
        downGears.Remove(gear);
        father.GetComponent<ThrowProjectile>().ReUpGear(gear);
    }
}
