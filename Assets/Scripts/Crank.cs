using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Crank : MonoBehaviour
{
    public List<Transform> downGears;

    public GameObject father;

    private void Awake() {
        ClearGears();
    }

    
    private void OnTriggerEnter() {

        Debug.Log("Cranking Up!");
        CrankUp();

    }

    public void CrankUp() {

        foreach(Transform gear in downGears) {

            gear.gameObject.GetComponent<drop>().isLocked = false;

        }

    }
    
    public void ClearGears() {

        downGears.Clear();
        father.GetComponent<ThrowProjectile>().Reshuffle();

    }
}
