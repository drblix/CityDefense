using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject missile;

    [SerializeField]
    private Transform missileContainer;

    [SerializeField]
    private Transform[] buildings;

    private bool gameOver = false;

    private int missilesLeft = 8;
    private float minWaitTime = 4f;
    private float maxWaitTime = 9f;

    private float missileSpeed = 25f;

    private const float xBound = 84f;
    private const float yLoc = 187f;

    #endregion

    private void Awake()
    {
        StartCoroutine(SpawnTask());
    }

    private IEnumerator SpawnTask()
    {
        for (int i = 0; i < missilesLeft; i++)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            if (gameOver) { break; }

            Vector3 spawnPos = new(Random.Range(-xBound, xBound), yLoc, 70f);
            GameObject newMis = Instantiate(missile, spawnPos, Quaternion.identity, missileContainer);
            newMis.transform.GetComponent<EvilMissile>().SetSettings(buildings[Random.Range(0, buildings.Length)], missileSpeed);
            newMis.name = "Missile" + (i + 1);
        }
    }
}
