using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private AudioClip[] explosionClips;

    public GameObject CreateExplosion(Vector3 position)
    {
        AudioClip clip = explosionClips[Random.Range(0, explosionClips.Length)];
        GameObject newExp = Instantiate(explosion, position, Quaternion.identity);
        AudioSource audSource = newExp.GetComponent<AudioSource>();
        audSource.clip = clip;
        audSource.Play();
        return newExp;
    }
}
