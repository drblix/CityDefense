using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    public GameObject CreateExplosion(Vector3 position)
    {
        GameObject newExp = Instantiate(explosion, position, Quaternion.identity);
        return newExp;
    }
}
