using Assets.AllyaExtension;
using UnityEngine;

public struct Shooter
{
    private const float RateOfFire = 800; // Rounds Per Minute
    private const float ImpulseForce = 100.0f;
    private const KeyCode ShootKey = KeyCode.Space;

    private readonly Transform player;
    private readonly IPositionSpawner spawner;
    private readonly Pool<Transform> pool;
    private readonly float fireInterval;
    private float lastShot;
    
    public Shooter(Transform projectilePrefab, Transform player, IPositionSpawner spawner)
    {
        this.player = player;
        this.spawner = spawner;
        pool = new Pool<Transform>(new ComponentCreator<Transform>(projectilePrefab), 30);
        lastShot = Time.time;
        fireInterval = 60.0f / RateOfFire;
    }
    
    public void Begin()
    {
        Scheduler.OnUpdate.Subscribe(OnUpdate);
    }

    private void OnUpdate()
    {
        if (lastShot + fireInterval > Time.time || !Input.GetKey(ShootKey))
        {
            return;
        }
        lastShot = Time.time;
        Transform projectileInstance = pool.Retrieve();
        SetComponents(projectileInstance);
        PoolReturner returner = new PoolReturner(pool, projectileInstance, spawner);
        returner.Begin();
        projectileInstance.GetComponent<Projectile>().Initialize(returner);
    }

    private void SetComponents(Component projectileInstance)
    {
        projectileInstance.gameObject.SetActive(true);
        Transform ply = player.transform;
        Transform proj = projectileInstance.transform;

        Vector3 dir = ply.up;
        proj.position = ply.position + dir * 2;
        proj.up = dir;
        Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();
        
        rb.velocity = dir * ImpulseForce;
        rb.angularVelocity = 0.0f;
    }
}