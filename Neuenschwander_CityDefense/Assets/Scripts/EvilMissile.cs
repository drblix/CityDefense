using System.Collections;
using UnityEngine;
using TMPro;

public class EvilMissile : MonoBehaviour
{
    #region Variables

    private RoundManager roundManager;
    private EnemySpawner enemSpawner;
    private ExplosionManager explosionMan;

    private Transform targetBuilding;
    private GameObject warningObj;

    private float misSpeed = 0f;
    private bool isDead = false;

    // Missile types
    private bool isSplitter = false;
    private bool isSpeedy = false;

    #endregion

    private void Awake()
    {
        enemSpawner = FindObjectOfType<EnemySpawner>();
        roundManager = FindObjectOfType<RoundManager>();
        explosionMan = FindObjectOfType<ExplosionManager>();

        warningObj = enemSpawner.CreateWarningObj();

        isSplitter = Random.Range(0f, 1f) < 0.1f;
        isSpeedy = Random.Range(0f, 1f) < 1f;

        if (isSplitter && !isSpeedy)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
            warningObj.GetComponent<TextMeshPro>().color = Color.yellow;

            TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
            trailRenderer.material.SetColor("_Color", Color.yellow);
            trailRenderer.endColor = Color.yellow;
            trailRenderer.startColor = Color.yellow;

            misSpeed = 10f;

            StartCoroutine(SplitterSequence());
        }
        else if (isSpeedy && !isSplitter)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            warningObj.GetComponent<TextMeshPro>().color = Color.green;

            TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
            trailRenderer.material.SetColor("_Color", Color.green);
            trailRenderer.endColor = Color.yellow;
            trailRenderer.startColor = Color.yellow;

            transform.position = new(transform.position.x, transform.position.y + 50f, transform.position.z);

            misSpeed = 50f;
        }
    }

    private void Update()
    {
        if (targetBuilding != null && misSpeed != 0f && !isDead)
        {
            print(misSpeed);
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, targetBuilding.position, misSpeed * Time.deltaTime), Quaternion.LookRotation(targetBuilding.position, transform.up));
        }
        else if (targetBuilding == null && !isDead && !enemSpawner.gameOver)
        {
            targetBuilding = enemSpawner.GetNewTarget();
        }

        if (transform.position.y > 174f)
        {
            Vector3 pos = new(transform.position.x, 163f, transform.position.z);
            warningObj.transform.position = pos;
        }
        else if (warningObj)
        {
            Destroy(warningObj);
        }
    }

    public void SetSettings(Transform trans, float spd)
    {
        targetBuilding = trans;

        if (!isSpeedy)
        {
            misSpeed = spd;
        }
    }


    public void MissileDeath()
    {
        isDead = true;
        GetComponent<BoxCollider>().enabled = false;
        GameObject exp = explosionMan.CreateExplosion(transform.position);

        if (isSplitter && !isSpeedy)
        {
            exp.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
        }
        else if (isSpeedy && !isSplitter)
        {
            exp.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
        }

        Destroy(transform.GetChild(0).gameObject);
        Destroy(this);
    }

    private IEnumerator SplitterSequence()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            enemSpawner.SpawnMissile(transform.position, 15f, "SplitMissile");
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Building"))
        {
            MissileDeath();

            int num;
            if (int.TryParse(collision.collider.name[8..], out int result))
            {
                num = result;
            }
            else
            {
                Debug.LogError("Failed to parse to integer");
                return;
            }

            roundManager.BuildingHit(num);
        }
    }
}
