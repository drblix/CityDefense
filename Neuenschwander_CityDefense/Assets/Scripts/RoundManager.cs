using System.Collections;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    private PlayerCursor pCursor;
    private EnemySpawner enemSpawner;

    [SerializeField]
    private GameObject[] buildings;
    [SerializeField]
    private GameObject[] buildingDebris;

    [SerializeField]
    private TextMeshProUGUI levelDisplay;
    [SerializeField]
    private TextMeshProUGUI missileDisplay;

    public int currentRound = 1;
    private int buildingsRemaining = 3;

    private void Awake()
    {
        enemSpawner = FindObjectOfType<EnemySpawner>();
        pCursor = FindObjectOfType<PlayerCursor>();

        enemSpawner.SetDataAndStart(6 + (currentRound * 2), Mathf.RoundToInt(1 + ((currentRound / 2) * 1.2f)));
        levelDisplay.SetText("Level: " + currentRound.ToString());
        missileDisplay.SetText("Missiles Left: 10");
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
        int numMis = 10 + (currentRound * 2);
        enemSpawner.SetDataAndStart(7 + (currentRound * 2), (int)(1 + ((currentRound / 2) * 1.2f)));
        pCursor.ResetPlayer(numMis);
        missileDisplay.SetText("Missiles Left: " + numMis.ToString());
        levelDisplay.SetText("Level: " + currentRound.ToString());
    }

    public void GameOver()
    {
        foreach (EvilMissile mis in FindObjectsOfType<EvilMissile>())
        {
            Destroy(mis.gameObject);
        }

        enemSpawner.gameOver = true;
    }

    public void BuildingHit(int num)
    {
        num--;
        GameObject blding = buildings[num];
        if (!blding) { return; }
        GameObject bldingFrag = Instantiate(buildingDebris[num], blding.transform.position, Quaternion.identity);
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
