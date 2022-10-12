using System.Collections;
using UnityEngine;

public class UFOScript : MonoBehaviour
{
    private EnemySpawner spawner;
    private ExplosionManager explosMan;

    private Vector3 goal = new(100f, 152f, 72f);

    private const float UFO_SPEED = 20f;
    private int missilesLeft;

    private void Awake()
    {
        explosMan = FindObjectOfType<ExplosionManager>();
        spawner = FindObjectOfType<EnemySpawner>();

        missilesLeft = Random.Range(2, 4);
        StartCoroutine(ShootMissile());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, Time.deltaTime * UFO_SPEED);

        if (transform.position.x >= goal.x)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ShootMissile()
    {
        for (int i = 0; i < missilesLeft; i++)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            spawner.SpawnMissile(transform.GetChild(0).position, 17.5f, "UFOMissile", false);
        }
    }

    public void UFODeath()
    {
        explosMan.CreateExplosion(transform.position);
        Destroy(gameObject);
    }
}
