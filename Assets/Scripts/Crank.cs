using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Crank : MonoBehaviour
{
    public List<Transform> downGears;

    public GameObject father;

    private void Awake()
    {
        downGears.Clear();
    }


    private void OnTriggerEnter()
    {
        Debug.Log("Cranking Up!");
        CrankUp();
    }

    public void CrankUp()
    {
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
