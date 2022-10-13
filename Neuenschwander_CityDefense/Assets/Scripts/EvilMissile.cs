using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Container for all enemy missile functions and behaviors
/// </summary>
public class EvilMissile : MonoBehaviour
{
    #region Variables

    private RoundManager roundManager;
    private EnemySpawner enemSpawner;
    private ExplosionManager explosionMan;

    [SerializeField]
    private Transform targetBuilding;
    private GameObject warningObj;

    private float misSpeed = 0f;
    private bool isDead = false;

    private bool canHaveTypes = false;

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
    }

    private void Update()
    {
        if (targetBuilding != null && misSpeed != 0f && !isDead)
        {
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

    public void SetSettings(Transform trans, float spd, bool haveTypes)
    {
        targetBuilding = trans;
        canHaveTypes = haveTypes;
        misSpeed = spd;
        
        if (canHaveTypes)
        {
            DetermineType();
        }

        if (isSplitter)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
            warningObj.GetComponent<TextMeshPro>().color = Color.yellow;

            TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
            trailRenderer.material.SetColor("_Color", Color.yellow);
            trailRenderer.endColor = Color.yellow;
            trailRenderer.startColor = Color.yellow;

            misSpeed = 14f;

            transform.name = "SplitterMissile";
            StartCoroutine(SplitterSequence());
        }
        else if (isSpeedy)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            warningObj.GetComponent<TextMeshPro>().color = Color.green;

            TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
            trailRenderer.material.SetColor("_Color", Color.green);
            trailRenderer.endColor = Color.yellow;
            trailRenderer.startColor = Color.yellow;

            transform.position = new(transform.position.x, transform.position.y + 50f, transform.position.z);

            misSpeed = 50f;

            transform.name = "SpeedMissile";
        }
    }


    public void MissileDeath(bool explos)
    {
        isDead = true;
        GetComponent<BoxCollider>().enabled = false;

        if (explos)
        {
            GameObject exp = explosionMan.CreateExplosion(transform.position);

            if (isSplitter)
            {
                exp.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
            }
            else if (isSpeedy)
            {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
                exp.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            }
        }

        Destroy(transform.GetChild(0).gameObject);
        Destroy(this);
    }

    private IEnumerator SplitterSequence()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));

        for (int i = 0; i < Random.Range(2, 4); i++)
        {
            enemSpawner.SpawnMissile(transform.position, 15f, "SplitMissile", false);
        }

        MissileDeath(false);
    }

    private void DetermineType()
    {
        float chance = 0.1f + (roundManager.currentRound / 40f);
        chance = Mathf.Clamp(chance, 0f, 0.6f);
        if (Random.Range(0f, 1f) < chance && roundManager.currentRound != 1)
        {
            int num = Random.Range(1, 3);

            switch (num)
            {
                case 1:
                    isSplitter = true;
                    break;
                case 2:
                    isSpeedy = true;
                    break;
                default:
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Building"))
        {

            int num;
            string c = collision.collider.name[8].ToString();
            if (int.TryParse(c, out int result))
            {
                num = result;
            }
            else
            {
                Debug.LogError("Failed to parse to integer");
                return;
            }

            roundManager.BuildingHit(num);
            MissileDeath(true);
        }
    }
}
