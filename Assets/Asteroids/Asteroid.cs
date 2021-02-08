using Systems;
using Asteroids;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public readonly struct Asteroid
{
    private const int MinVertices = 5;
    private const int MaxVertices = 16;
    private const int InitialHealth = 4;
    private const float HealthSizeProportion = 2.71f;

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
            RandomUtils.RandomConvexPolygon(Random.Range(MinVertices, MaxVertices), health / HealthSizeProportion);
        gameObject = ObjectUtils.CreatePolygonalObject("Asteroid", Layers.Asteroid,  polygons, 0.1f);
        gameObject.transform.position = position;
        AddComponents();
        polygons.Dispose();
    }

    public void Subdivide()
    {
        Vector3 pos = gameObject.transform.position;

        gameObject.transform.position = spawner.Position();
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        AsteroidUtils.ApplyImpulse(rb);
        
        if (health <= 0)
        {
            return;
        }

        CreateAsteroid(spawner, health - 1, pos);
        CreateAsteroid(spawner, health - 1, pos);
    }

    private void AddComponents()
    {
        AsteroidUtils.ApplyImpulse(gameObject.GetComponent<Rigidbody2D>());
        
        /* Collision Notification */
        AsteroidCollision collision = gameObject.AddComponent<AsteroidCollision>();
        collision.Initialize(this);
        
        /* Reallocator */
        SystemManager.RegisterSystem(new AsteroidReallocator(gameObject.transform, spawner), 
            new UnityEventBind(GameEvents.GameOver));

        GameObject circleCollider = new GameObject("Circle Collider")
        {
            layer = Layers.SteeringAvoidance
        };
        CircleCollider2D collider = circleCollider.AddComponent<CircleCollider2D>();
        collider.radius = health / HealthSizeProportion * RandomUtils.MaxVertexDisplacement + 0.5f;
        circleCollider.transform.SetParent(gameObject.transform);
        circleCollider.transform.localPosition = Vector3.zero;
    }
}