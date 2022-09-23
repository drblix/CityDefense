using UnityEngine;

public class EvilMissile : MonoBehaviour
{
    #region Variables

    private Explosion explosion;
    private Transform targetBuilding;
    private float misSpeed = 0f;

    #endregion

    private void Awake()
    {
        explosion = FindObjectOfType<Explosion>();
    }

    private void Update()
    {
        if (targetBuilding != null && misSpeed != 0f)
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
            GetComponent<BoxCollider>().enabled = false;
            explosion.CreateExplosion("good", transform.position);
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
