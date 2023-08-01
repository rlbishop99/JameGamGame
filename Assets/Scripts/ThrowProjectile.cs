using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : MonoBehaviour
{

    [Header("Projectiles")]
    public GameObject gearProjectile;
    public GameObject enemyProjectile;
    
    [Header("Throwing Info")]
    public Transform endPoint = null;
    public Transform startPos;

    private  Coroutine co;
    private int i = 0;

    private int projectileType;
    private Transform enemyPoint;

    [Header("Transform Arrays")]
    public Transform[] hitPoints;
    public Transform[] shuffledArray;

    private void Awake() {
        shuffledArray = Shuffle(hitPoints);
    }
    
    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsPlaying()) {
            co = StartCoroutine(LobProjectile());
        }
    }

    private void Update() {
        if (i == shuffledArray.Length && co != null) {
            StopCoroutine(co);
        }
    }

    IEnumerator LobProjectile() {

        while(true) {

            projectileType = Random.Range(1,3);

            
            if(projectileType == 1) {

                GetNewHitPoint(i);
                gearProjectile.GetComponent<ProjectileMovement>().AssignStart(startPos);
                gearProjectile.GetComponent<ProjectileMovement>().AssignEnd(endPoint);
                Instantiate(gearProjectile, startPos.position, Quaternion.identity);
                i++;

            }

            if(projectileType == 2) {

                GetNewEnemyPoint(i);
                enemyProjectile.GetComponent<ProjectileMovement>().AssignStart(startPos);
                enemyProjectile.GetComponent<ProjectileMovement>().AssignEnd(enemyPoint);
                Instantiate(enemyProjectile, startPos.position, Quaternion.identity);

            }

            yield return new WaitForSeconds(4f);

        }
        
    }

    private void GetNewHitPoint(int index) {

        endPoint = shuffledArray[i];

    }

    private void GetNewEnemyPoint(int index) {

        enemyPoint = shuffledArray[Random.Range(index, shuffledArray.Length)];

    }

    public Transform[] Shuffle(Transform[] array)
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

        return shuffledArray;
    }

    public void Reshuffle() {

        StopAllCoroutines();
        Shuffle(hitPoints);
        if (GameManager.Instance != null && GameManager.Instance.IsPlaying()) {
            co = StartCoroutine(LobProjectile());
            Debug.Log("Reshuffled and Restarted Coroutine!");
        }

    }
}
