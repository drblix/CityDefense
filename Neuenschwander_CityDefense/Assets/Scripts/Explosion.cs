using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
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
            if (collision.collider.CompareTag("Missile"))
            {
                EvilMissile evilMis = collision.collider.GetComponent<EvilMissile>();
                if (evilMis)
                {
                    evilMis.MissileDeath(true);
                }
            }
            else if (collision.collider.CompareTag("UFO"))
            {
                UFOScript ufoS = collision.collider.GetComponent<UFOScript>();
                if (ufoS)
                {
                    ufoS.UFODeath();
                }
            }
        }
    }
}
