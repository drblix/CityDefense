using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    private GameObject warningSymb;

    [SerializeField]
    private Transform missileContainer;

    public List<Transform> bldings = new(3);

    public bool gameOver = false;

    private int missilesLeft = 6;
    private int ufoLeft = 1;

    private float minWaitTime = 4f;
    private float maxWaitTime = 9f;

    private const float xBound = 84f;
    private const float yLoc = 200f;

    #endregion

    private void Awake()
    {
        roundManager = FindObjectOfType<RoundManager>();
    }

    private IEnumerator SpawnTask()
    {
        if (roundManager.currentRound != 1)
        {
            float modi = Random.Range(0.2f, 0.6f);
            minWaitTime -= modi;
            maxWaitTime -= modi;
            minWaitTime = Mathf.Clamp(minWaitTime, 0f, 4f);
            maxWaitTime = Mathf.Clamp(maxWaitTime, 2f, 9f);
        }

        for (int i = 0; i < missilesLeft; i++)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            if (gameOver) { break; }

            Vector3 spawnPos;

            if (Random.Range(0f, 1f) > 0.2f || ufoLeft == 0)
            {
                spawnPos = new(Random.Range(-xBound, xBound), yLoc, 70f);
                SpawnMissile(spawnPos, 25f, "Missile" + (i + 1));
            }
            else if (FindObjectsOfType<UFOScript>().Length == 0)
            {
                ufoLeft--;
                spawnPos = new(-100f, 152f, 72f);
                Instantiate(ufo, spawnPos, Quaternion.identity);
            }
            else
            {
                spawnPos = new(Random.Range(-xBound, xBound), yLoc, 70f);
                SpawnMissile(spawnPos, 25f, "Missile" + (i + 1));
            }
        }

        StartCoroutine(roundManager.OutOfMissiles());
    }

    public void SpawnMissile(Vector3 pos, float speed, string name)
    {
        GameObject newMis = Instantiate(missile, pos, Quaternion.identity, missileContainer);

        Transform bldingTarget = bldings[Random.Range(0, bldings.Count)];

        newMis.transform.GetComponent<EvilMissile>().SetSettings(bldingTarget, speed);
        newMis.name = name;
    }

    public void SetDataAndStart(int numMis, int numUfo)
    {
        missilesLeft = numMis;
        ufoLeft = numUfo;
        StartCoroutine(SpawnTask());
    }

    public Transform GetNewTarget()
    {
        Transform blding = bldings[Random.Range(0, bldings.Count)];
        
        if (blding) { return blding; }
        else { return null; }
    }

    public GameObject CreateWarningObj()
    {
        return Instantiate(warningSymb);
    }
}
