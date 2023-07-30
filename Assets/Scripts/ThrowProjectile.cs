using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : MonoBehaviour
{

    public GameObject projectile;
    public float timeBetweenThrows;

    public Transform[] hitPoints;
    public Transform endPoint;

    public Transform startPos;

    private float startTime = 2f;
    private bool hasStarted = false;

    private void Update() {
        
        startTime -= Time.deltaTime;

        if(startTime <= 0 && hasStarted == false) {

            StartCoroutine(LobProjectile());
            hasStarted = true;

        }

    }

    IEnumerator LobProjectile() {

        while(true){

            Debug.Log("Lobbing another Projectile");
            GetNewHitPoint();
            Instantiate(projectile, startPos.position, Quaternion.identity);
            projectile.GetComponent<ProjectileMovement>().AssignStart(startPos);
            projectile.GetComponent<ProjectileMovement>().AssignEnd(endPoint);
            yield return new WaitForSeconds(3f);

        }
        
    }

    private void GetNewHitPoint() {

        endPoint = hitPoints[Random.Range(0, hitPoints.Length - 1)].transform;

    }
}
