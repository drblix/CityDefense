using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private RoundManager roundManager;
    private Animator animator;

    private void Awake()
    {
        roundManager = FindObjectOfType<RoundManager>();
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
                roundManager.ChangeScore(100);
            }
            else if (collision.collider.CompareTag("UFO"))
            {
                UFOScript ufoS = collision.collider.GetComponent<UFOScript>();
                if (ufoS)
                {
                    ufoS.UFODeath();
                }
                roundManager.ChangeScore(250);
            }
        }
    }
}
