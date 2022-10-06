using System.Collections;
using UnityEngine;

public class UFOScript : MonoBehaviour
{
    private EnemySpawner spawner;
    private ExplosionManager explosMan;

    private Vector3 goal = new(100f, 152f, 72f);

    private float ufoSpeed = 20f;
    private int missilesLeft;

    private void Awake()
    {
        spawner = FindObjectOfType<EnemySpawner>();
        missilesLeft = Random.Range(2, 5);
        StartCoroutine(ShootMissile());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, Time.deltaTime * ufoSpeed);

        if (transform.position.x >= goal.x)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ShootMissile()
    {
        for (int i = 0; i < missilesLeft; i++)
        {
            spawner.SpawnMissile(transform.GetChild(0).position);
            yield return new WaitForSeconds(Random.Range(3f, 6f));
        }
    }


    public void UFODeath()
    {
        explosMan.CreateExplosion(transform.position);
        Destroy(gameObject);
    }
}
