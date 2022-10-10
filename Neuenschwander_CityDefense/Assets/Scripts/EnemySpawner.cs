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
    private Transform[] buildings = new Transform[3];

    public bool gameOver = false;

    private int missilesLeft = 6;
    private int ufoLeft = 1;

    private const float MIN_WAIT_TIME = 4f;
    private const float MAX_WAIT_TIME = 9f;

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
            yield return new WaitForSeconds(Random.Range(MIN_WAIT_TIME, MAX_WAIT_TIME));

            if (gameOver) { break; }

            Vector3 spawnPos;

            if (Random.Range(0f, 1f) > 1.1f || ufoLeft == 0)
            {
                spawnPos = new(Random.Range(-xBound, xBound), yLoc, 70f);
                SpawnMissile(spawnPos, 25f, "Missile" + (i + 1));
            }
            else
            {
                ufoLeft--;
                spawnPos = new(-100f, 152f, 72f);
                Instantiate(ufo, spawnPos, Quaternion.identity);
            }
        }

        StartCoroutine(roundManager.OutOfMissiles());
    }

    public void SpawnMissile(Vector3 pos, float speed, string name)
    {
        GameObject newMis = Instantiate(missile, pos, Quaternion.identity, missileContainer);
        newMis.transform.GetComponent<EvilMissile>().SetSettings(buildings[Random.Range(0, buildings.Length)], speed);
        newMis.name = name;
    }

    public void SetDataAndStart(int numMis, int numUfo)
    {
        missilesLeft = numMis;
        ufoLeft = numUfo;
        StartCoroutine(SpawnTask());
    }
}
