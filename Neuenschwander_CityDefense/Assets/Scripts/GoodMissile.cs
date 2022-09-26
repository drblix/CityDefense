using UnityEngine;

public class GoodMissile : MonoBehaviour
{
    private ExplosionManager explosionManager;

    public Vector3 gotoPos = Vector3.zero;

    private const float MISSILE_SPEED = 50f;

    private bool done = false;

    private void Awake()
    {
        explosionManager = FindObjectOfType<ExplosionManager>();
    }

    private void Update()
    {
        if (gotoPos != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, gotoPos, MISSILE_SPEED * Time.deltaTime);
            if (transform.position == gotoPos && !done)
            {
                done = true;
                MovementFinished();
            }
        }            
    }

    private void MovementFinished()
    {
        Destroy(transform.GetChild(0).gameObject);
        GameObject newExp = explosionManager.CreateExplosion(gotoPos);
        newExp.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
    }
}
