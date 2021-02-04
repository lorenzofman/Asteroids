using Asteroids;
using Unity.Collections;
using UnityEngine;

public readonly struct Asteroid
{
    private const int MinVertices = 5;
    private const int MaxVertices = 16;
    private const int InitialHealth = 4;
    private readonly IPositionSpawner spawner;
    private readonly int health;
    private readonly GameObject gameObject;

    public static void CreateAsteroid(IPositionSpawner spawner)
    {
        CreateAsteroid(spawner, InitialHealth, spawner.Position());
    }

    private static void CreateAsteroid(IPositionSpawner spawner, int health, Vector3 position)
    {
        Asteroid _ = new Asteroid(spawner, health, position);
    }

    private Asteroid(IPositionSpawner spawner, int health, Vector3 position)
    {
        this.spawner = spawner;
        this.health = health;
        NativeArray<Vector2> polygons =
            RandomUtils.RandomConvexPolygon(Random.Range(MinVertices, MaxVertices), health / 2.71f);
        gameObject = ObjectUtils.CreatePolygonalObject("Asteroid", polygons, 0.1f, position);
        AddComponents();
        polygons.Dispose();
    }

    public void Subdivide()
    {
        Vector3 pos = gameObject.transform.position;

        gameObject.transform.position = spawner.Position();
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.angularVelocity = 0.0f;
        rb.velocity = Vector2.zero;
        
        if (health <= 0)
        {
            return;
        }

        CreateAsteroid(spawner, health - 1, pos);
        CreateAsteroid(spawner, health - 1, pos);
    }

    private void AddComponents()
    {
        /* Random impulse */
        gameObject.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle, ForceMode2D.Impulse);
        
        /* Collision Notification */
        AsteroidCollision collision = gameObject.AddComponent<AsteroidCollision>();
        collision.Initialize(this);
        
        /* Reallocator */
        AsteroidReallocator asteroidReallocator = new AsteroidReallocator(gameObject.transform, spawner);
        asteroidReallocator.Begin();
    }
}