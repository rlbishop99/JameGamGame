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

    public Animator anim;

    [Header("Coroutine Info")]
    public float timeBetweenWaves;
    private int lobCounter;
    private Coroutine co;
    private int projectileType;

    [Header("Transform Arrays")]
    public List<Transform> hitPoints;

    private void Start()
    {
        timeBetweenWaves = 4f;
        lobCounter = 0;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsPlaying())
        {
            co = StartCoroutine(LobProjectile());
        }
    }

    IEnumerator LobProjectile()
    {

        while (true)
        {

            switch(lobCounter) {

            case 3:
                timeBetweenWaves = 3f;
                PlayerController.Instance.healthGain = 5f;
                break;
            case 9:
                PlayerController.Instance.healthGain = 4f;
                timeBetweenWaves = 2f;
                break;
            case 22:
                PlayerController.Instance.healthGain = 3f;
                timeBetweenWaves = 1f;
                break;
            case 35:
                PlayerController.Instance.healthGain = 2f;
                timeBetweenWaves = .8f;
                break;
            default:
                break;

            }

            if (hitPoints.Count > 0)
            {
                projectileType = Random.Range(1, 11);
                endPoint = GetPoint();

                if (projectileType <=4 )
                {

                    gearProjectile.GetComponent<ProjectileMovement>().AssignStart(startPos);
                    gearProjectile.GetComponent<ProjectileMovement>().AssignEnd(endPoint);
                    Instantiate(gearProjectile, startPos.position, Quaternion.identity);
                    anim.SetTrigger("Throwing");
                    hitPoints.Remove(endPoint);
                } else {

                    enemyProjectile.GetComponent<ProjectileMovement>().AssignStart(startPos);
                    enemyProjectile.GetComponent<ProjectileMovement>().AssignEnd(endPoint);
                    anim.SetTrigger("Throwing");
                    Instantiate(enemyProjectile, startPos.position, Quaternion.identity);

                }
            }

            lobCounter++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private Transform GetPoint()
    {
        return hitPoints[Random.Range(0, hitPoints.Count)];
    }

    public void ReUpGear(Transform gear)
    {
        hitPoints.Add(gear);
    }
}
