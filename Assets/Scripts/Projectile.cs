using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float speed = 1;

    private float rotationAllowedTimer;

    float randStartTimer;
    float randX;
    float randY;

    // Start is called before the first frame update
    void Start()
    {
        rotationAllowedTimer = 0;
        PickRandomisedVariables();
        RandomizeRotation();
        RandomizeXY();
    }

    void IncrementRotationAllowedTimer()
    {
        rotationAllowedTimer += Time.deltaTime;
    }

    void PickRandomisedVariables()
    {
        float randomX = GameManager.instance.randomXDist;
        randStartTimer = Random.Range(0, .04f);
        randX = Random.Range(transform.position.x - randomX, transform.position.x + randomX);
        randY = Random.Range(transform.position.y - randomX, transform.position.y + randomX);
    }

    void RandomizeRotation()
    {
        float rand = Random.Range(-360, 361);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rand);
    }

    void RandomizeXY()
    {
        transform.localPosition = new Vector3(randX, randY, 0);
    }

    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        IncrementRotationAllowedTimer();
        MoveForward();

        if (target != null)
            LookAtTarget(target);
    }

    public void LookAtTarget(Transform newTarget)
    {
        target = newTarget;

        Vector3 difference = newTarget.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ+90);

        //transform.LookAt(target);
    }

    public void MoveForward()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (CheckDistanceToTarget(target))
            HitTarget();
        else
        {
            transform.Translate(-transform.up * (Time.deltaTime * speed));
        }

    }

    bool CheckDistanceToTarget(Transform target)
    {
        if (Vector2.Distance(transform.position, target.position) <= GameManager.instance.minProjectileKillDist)
        {
            return true;
        }
        else
            return false;
    }

    void HitTarget()
    {
        Destroy(gameObject);
    }
}
