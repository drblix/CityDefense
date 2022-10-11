using UnityEngine;

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
        else if (targetBuilding == null)
        {
            targetBuilding = enemSpawner.GetNewTarget();
        }

        if (transform.position.y > 174f)
        {
            Debug.Log("place exclamation point");
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
        misSpeed = spd;
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

    public void MissileDeath()
    {
        isDead = true;
        GetComponent<BoxCollider>().enabled = false;
        explosionMan.CreateExplosion(transform.position);
        Destroy(transform.GetChild(0).gameObject);
    }
}
