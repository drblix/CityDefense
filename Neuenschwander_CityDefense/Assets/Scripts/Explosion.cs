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
        if (collision.collider.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);
        }
    }
}
