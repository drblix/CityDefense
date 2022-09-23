using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    private string explosionType = "";

    public void CreateExplosion(string type, Vector3 position)
    {
        GameObject newExp = Instantiate(explosion, position, Quaternion.identity);
        StartCoroutine(newExp.GetComponent<Explosion>().DestroyDelay());
        if (type == "good")
        {
            newExp.name = "GoodExplosion";
            explosionType = type;
        }
        else if (type == "bad")
        {
            newExp.name = "BadExplosion";
            explosionType = type;
        }
        else
        {
            newExp.name = "DummyExplosion";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Missile") && explosionType != "")
        {
            if (explosionType == "good" && collision.collider.name.ToLower().Contains("evil"))
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
