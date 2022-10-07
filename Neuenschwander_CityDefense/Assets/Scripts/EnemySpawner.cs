using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Variables

    private RoundManager roundManager;

    [SerializeField]
    private GameObject missile;
    [SerializeField]
    private GameObject ufo;

    [SerializeField]
    private Transform missileContainer;

    [SerializeField]
    private Transform[] buildings;

    public bool gameOver = false;

    private int missilesLeft = 6;
    private int ufoLeft = 1;

    private float minWaitTime = 4f;
    private float maxWaitTime = 9f;

    private float missileSpeed = 25f;

    private const float xBound = 84f;
    private const float yLoc = 187f;

    #endregion

    private void Awake()
    {
        roundManager = FindObjectOfType<RoundManager>();
        StartCoroutine(SpawnTask());
    }

    private IEnumerator SpawnTask()
    {
        for (int i = 0; i < missilesLeft; i++)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            if (gameOver) { break; }

            Vector3 spawnPos;
            GameObject newEnemy;

            if (Random.Range(0f, 1f) > 1.1f || ufoLeft == 0)
            {
                spawnPos = new(Random.Range(-xBound, xBound), yLoc, 70f);
                newEnemy = Instantiate(missile, spawnPos, Quaternion.identity, missileContainer);
                newEnemy.transform.GetComponent<EvilMissile>().SetSettings(buildings[Random.Range(0, buildings.Length)], missileSpeed);
                newEnemy.name = "Missile" + (i + 1);
            }
            else
            {
                ufoLeft--;
                spawnPos = new(-100f, 152f, 72f);
                Instantiate(ufo, spawnPos, Quaternion.identity);
            }
        }

        roundManager.
    }

    public void SpawnMissile(Vector3 pos)
    {
        GameObject newMis = Instantiate(missile, pos, Quaternion.identity, missileContainer);
        newMis.transform.GetComponent<EvilMissile>().SetSettings(buildings[Random.Range(0, buildings.Length)], missileSpeed);
        newMis.name = "UFOMissile";
    }

    public void SetDataAndStart(int numMis, int numUfo)
    {
        missilesLeft = numMis;
        ufoLeft = numUfo;
        StartCoroutine(SpawnTask());
    }
}
