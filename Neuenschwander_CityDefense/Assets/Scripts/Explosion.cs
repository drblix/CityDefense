using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.name.Contains("Good"))
        {
            print("tick");
            if (collision.collider.CompareTag("Missile"))
            {
                EvilMissile evilMis = collision.collider.GetComponent<EvilMissile>();
                evilMis.MissileDeath();
            }
            else if (collision.collider.CompareTag("UFO"))
            {
                UFOScript ufoS = collision.collider.GetComponent<UFOScript>();
                ufoS.UFODeath();
            }
        }
    }
}
