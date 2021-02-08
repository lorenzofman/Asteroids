using Systems;
using UnityEngine;

public class Shooter
{
    private const float ImpulseForce = 50.0f;
    private readonly Transform shooterTransform;
    private readonly IPositionSpawner spawner;
    private readonly Pool<Transform> pool;
        
    private readonly float fireInterval;
    private float lastShot;
    
    public Shooter(Transform projectilePrefab, Transform shooterTransform, IPositionSpawner spawner, int rateOfFire)
    {
        this.shooterTransform = shooterTransform;
        this.spawner = spawner;
        pool = new Pool<Transform>(new ComponentCreator<Transform>(projectilePrefab), 30);
        lastShot = Time.time;
        fireInterval = 60.0f / rateOfFire;
    }

    public void Shoot()
    {
        if (lastShot + fireInterval > Time.time)
        {
            return;
        }
        lastShot = Time.time;
        Transform projectileInstance = pool.Retrieve();
        SetComponents(projectileInstance);
        PoolReturner returner = new PoolReturner(pool, projectileInstance, spawner);
        SystemManager.RegisterSystem(returner, null, -1);
        projectileInstance.GetComponent<Projectile>().Initialize(returner);
    }

    private void SetComponents(Component projectileInstance)
    {
        projectileInstance.gameObject.SetActive(true);
        Transform ply = shooterTransform.transform;
        Transform proj = projectileInstance.transform;

        Vector3 dir = ply.up;
        proj.position = ply.position + dir * 2;
        proj.up = dir;
        Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();
        
        rb.velocity = dir * ImpulseForce;
        rb.angularVelocity = 0.0f;
    }
}