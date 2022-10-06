using UnityEngine;

public class EvilMissile : MonoBehaviour
{
    #region Variables

    private ExplosionManager explosionMan;
    private Transform targetBuilding;
    
    private float misSpeed = 0f;
    private bool isDead = false;

    #endregion

    private void Awake()
    {
        explosionMan = FindObjectOfType<ExplosionManager>();
    }

    private void Update()
    {
        if (targetBuilding != null && misSpeed != 0f && !isDead)
        {
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, targetBuilding.position, misSpeed * Time.deltaTime), Quaternion.LookRotation(targetBuilding.position, transform.up));
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
