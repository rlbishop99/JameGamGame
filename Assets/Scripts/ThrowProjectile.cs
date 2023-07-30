using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : MonoBehaviour
{

    public GameObject projectile;
    public float timeBetweenThrows;
    public Transform endPoint = null;
    public Transform startPos;

    private float startTime = 2f;
    private bool hasStarted = false;

    private  Coroutine co;
    private int i = 0;

    public Transform[] hitPoints;
    public Transform[] shuffledArray;

    private void Awake() {
        shuffledArray = Shuffle(hitPoints);
    }

    private void Update() {
        
        startTime -= Time.deltaTime;

        if(startTime <= 0 && hasStarted == false) {

            hasStarted = true;
            co = StartCoroutine(LobProjectile());

        }

        if(i == shuffledArray.Length) {

            Debug.Log("Stopping Coroutine");
            StopCoroutine(co);

        }

    }

    IEnumerator LobProjectile() {

        while(true && i < shuffledArray.Length){

            GetNewHitPoint(i);
            projectile.GetComponent<ProjectileMovement>().AssignStart(startPos);
            projectile.GetComponent<ProjectileMovement>().AssignEnd(endPoint);
            Instantiate(projectile, startPos.position, Quaternion.identity);
            Debug.Log("Lobbing another Projectile at: " + endPoint.position + " This is at index: " + i);
            i++;
            yield return new WaitForSeconds(4f);

        }
        
    }

    private void GetNewHitPoint(int index) {

        endPoint = shuffledArray[i];

    }

    public static Transform[] Shuffle(Transform[] array)
    {
        Transform[] shuffledArray = new Transform[array.Length];
        array.CopyTo(shuffledArray, 0);

        int n = shuffledArray.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Transform value = shuffledArray[k];
            shuffledArray[k] = shuffledArray[n];
            shuffledArray[n] = value;
        }


        for(int j = 0; j < shuffledArray.Length; j++)
        {

            Debug.Log(shuffledArray[j].position);

        }

        return shuffledArray;
    }
}
