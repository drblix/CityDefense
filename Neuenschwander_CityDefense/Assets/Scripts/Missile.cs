using UnityEngine;

public class Missile : MonoBehaviour
{
    #region Variables

    private Transform targetBuilding;
    private float misSpeed = 0f;

    #endregion

    private void Update()
    {
        if (targetBuilding != null && misSpeed != 0f)
        {
            transform.rotation = Quaternion.LookRotation(targetBuilding.position, transform.up);
            transform.position = Vector3.MoveTowards(transform.position, targetBuilding.position, misSpeed * Time.deltaTime);
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

            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
