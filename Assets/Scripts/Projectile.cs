using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int spawnRotation;
    [SerializeField] private bool allowRandomSpawnRotation;
    [SerializeField] private bool allowRandomSpawnPosition;
    [SerializeField] private Image projectileImage;
    [SerializeField] private UIElement projectileSpinImageUI;
    private Color projectileInvisColor;
    private bool allowAnimate;
    [SerializeField] private float spinSpeed;

    private Transform target;

    private float speed = 1;

    private float rotationAllowedTimer;

    float randStartTimer;
    float randX;
    float randY;

    bool isEnemyProjectile;

    bool spinningLeft;
    private Animator animator;

    public void UpdateProjectileSprite(Sprite sprite)
    {
        //Debug.Log(sprite.name);
        this.projectileImage.sprite = sprite;
    }

    public void UpdateProjectileInvisColour(Color colour)
    {
        projectileInvisColor = colour;
    }

    public void UpdateProjectileAnimator(RuntimeAnimatorController ac)
    {
        if (ac == null)
            return;

        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = ac;
    }

    // Start is called before the first frame update

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Flip sprite 
        if (allowAnimate)
        {
            int rand = Random.Range(0, 2);
            if (rand == 1)
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            else
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        }
    }

    void Start()
    {
        rotationAllowedTimer = 0;
        PickRandomisedVariables();

        if (allowRandomSpawnRotation)
            RandomizeRotation();

        if (allowRandomSpawnPosition) 
            RandomizeXY();
    }

    void IncrementRotationAllowedTimer()
    {
        rotationAllowedTimer += Time.deltaTime;
    }

    void PickRandomisedVariables()
    {
        float randomX = GameManager.Instance.randomXDist;
        randStartTimer = Random.Range(0, .04f);
        randX = Random.Range(transform.position.x - randomX, transform.position.x + randomX);
        randY = Random.Range(transform.position.y - randomX, transform.position.y + randomX);
    }

    void RandomizeRotation()
    {
        float rand = Random.Range(-spawnRotation, spawnRotation);
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
        //Spin();

        // If projectile has a target, and animation is not playing, look at target
        if (target != null)
            LookAtTarget(target);
    }

    public void ToggleAllowSpin(bool toggle)
    {
        allowAnimate = toggle;

        if (allowAnimate)
        {
            animator = GetComponent<Animator>();
            animator.SetTrigger("spin");
        }
    }

    public void ToggleIdle(bool toggle)
    {
        allowAnimate = toggle;

        if (allowAnimate)
        {
            animator = GetComponent<Animator>();
            animator.SetTrigger("idle");
        }
    }

    public void LookAtTarget(Transform newTarget)
    {
        target = newTarget;

        Vector3 difference = newTarget.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotationZ+90);

        //transform.LookAt(target);
    }

    bool GetTeam()
    {
        return isEnemyProjectile;
    }

    public void UpdateTeam(bool toggle)
    {
        isEnemyProjectile = toggle;
    }
    public void MoveForward()
    {
        // If there is no target for whatever reason, destroy the projectile
        if (target == null)
            HitTarget();

        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (CheckDistanceToTarget(target))
            HitTarget();
        else
        {
            if (!GetTeam())
                transform.Translate(transform.up * (Time.deltaTime * speed));
            else
                transform.Translate(-transform.up * (Time.deltaTime * speed));
        }
    }

    bool CheckDistanceToTarget(Transform target)
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) <= GameManager.Instance.minProjectileKillDist)
            {
                return true;
            }
            else
                return false;
        }

        return true;
    }

    void HitTarget()
    {
        Destroy(gameObject);
    }
}
