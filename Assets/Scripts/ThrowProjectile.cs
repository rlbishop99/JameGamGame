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

    private Coroutine co;
    private int projectileType;

    [Header("Transform Arrays")]
    public List<Transform> hitPoints;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsPlaying())
        {
            co = StartCoroutine(LobProjectile());
        }
    }

    private void Update()
    {

    }

    IEnumerator LobProjectile()
    {
        while (true)
        {
            if (hitPoints.Count > 0)
            {
                projectileType = Random.Range(1, 3);
                endPoint = GetPoint();

                if (projectileType == 1)
                {

                    gearProjectile.GetComponent<ProjectileMovement>().AssignStart(startPos);
                    gearProjectile.GetComponent<ProjectileMovement>().AssignEnd(endPoint);
                    Instantiate(gearProjectile, startPos.position, Quaternion.identity);
                    hitPoints.Remove(endPoint);
                }

                if (projectileType == 2)
                {
                    enemyProjectile.GetComponent<ProjectileMovement>().AssignStart(startPos);
                    enemyProjectile.GetComponent<ProjectileMovement>().AssignEnd(endPoint);
                    Instantiate(enemyProjectile, startPos.position, Quaternion.identity);
                }
            }

            yield return new WaitForSeconds(4f);
        }
    }

    private Transform GetPoint()
    {
        return hitPoints[Random.Range(0, hitPoints.Count)];
    }

    public void ReUpGear(Transform gear)
    {
        hitPoints.Add(gear);
        Debug.Log("Readded Gear!");
    }
}
