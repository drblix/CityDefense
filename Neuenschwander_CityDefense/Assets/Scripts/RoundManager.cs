using System.Collections;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    private PlayerCursor pCursor;
    private EnemySpawner enemSpawner;

    private Transform misContainer;
    private Transform debrisContainer;

    [SerializeField]
    private GameObject[] buildings;

    [SerializeField]
    private GameObject[] bldingPfabs;
    [SerializeField]
    private GameObject[] buildingDebris;

    [SerializeField]
    private Vector3[] bldingPositions;

    [SerializeField]
    private TextMeshProUGUI levelDisplay;
    [SerializeField]
    private TextMeshProUGUI missileDisplay;
    [SerializeField]
    private TextMeshProUGUI scoreDisplay;
    [SerializeField]
    private TextMeshProUGUI gameOverTxt;

    public int currentRound = 1;
    private int buildingsRemaining = 3;

    private int score = 0;
    private int highScore = 0;

    private void Awake()
    {
        enemSpawner = FindObjectOfType<EnemySpawner>();
        pCursor = FindObjectOfType<PlayerCursor>();

        misContainer = GameObject.Find("MissileContainer").transform;
        debrisContainer = GameObject.Find("DebrisContainer").transform;   
    }

    public IEnumerator OutOfMissiles()
    {
        while (FindObjectsOfType<EvilMissile>().Length > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        LoadNextRound();
    }

    private void LoadNextRound()
    {
        currentRound++;
        int numMis = 10 + (currentRound * 3);
        enemSpawner.SetDataAndStart(7 + (currentRound * 2), (int)(1 + ((currentRound / 2) * 1.2f)));
        pCursor.ResetPlayer(numMis);
        missileDisplay.SetText("Missiles \nLeft: " + numMis.ToString());
        levelDisplay.SetText("Level: " + currentRound.ToString());
    }

    public void GameOver()
    {
        foreach (EvilMissile mis in FindObjectsOfType<EvilMissile>())
        {
            Destroy(mis.gameObject);
        }

        foreach (Transform child in misContainer)
        {
            Destroy(child.gameObject);
        }

        enemSpawner.gameOver = true;
        pCursor.gameOver = true;
        gameOverTxt.gameObject.SetActive(true);

        highScore = score > highScore ? score : highScore;
    }

    public void ResetEverything()
    {
        gameOverTxt.gameObject.SetActive(false);
        currentRound = 1;
        score = 0;

        for (int i = 0; i < bldingPositions.Length; i++)
        {
            buildings[i] = Instantiate(bldingPfabs[i], bldingPositions[i], Quaternion.identity);

            if (i == 0)
            {
                buildings[i].transform.rotation = Quaternion.Euler(0, -90f, 0);
            }
            else
            {
                buildings[i].transform.rotation = Quaternion.Euler(0, 180f, 0);
            }

            enemSpawner.bldings.Add(buildings[i].transform);
        }

        foreach (Transform child in debrisContainer)
        {
            Destroy(child.gameObject);
        }

        enemSpawner.SetDataAndStart(6, 1);
        pCursor.ResetPlayer(10);
        levelDisplay.SetText("Level: " + currentRound.ToString());
        missileDisplay.SetText("Missiles \nLeft: 10");
        gameOverTxt.gameObject.SetActive(false);
        enemSpawner.gameOver = false;
        pCursor.gameOver = false;
    }

    public void BuildingHit(int num)
    {
        num--;
        GameObject blding = buildings[num];
        if (!blding) { return; }
        GameObject bldingFrag = Instantiate(buildingDebris[num], blding.transform.position, Quaternion.identity, debrisContainer);
        enemSpawner.bldings.Remove(blding.transform);
        Destroy(blding);

        foreach (Transform child in bldingFrag.transform)
        {
            child.GetComponent<Rigidbody>().AddExplosionForce(1000f, bldingFrag.transform.position, 20f);
        }

        buildingsRemaining--;
        if (buildingsRemaining == 0)
        {
            GameOver();
        }
    }
}
