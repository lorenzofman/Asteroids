using System.Collections.Generic;
using Systems;
using Enemies;
using Projectiles;
using Unity.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform projectilePrefab;
    [SerializeField] private KeyCode leftKeyCode;
    [SerializeField] private KeyCode rightKeyCode;
    [SerializeField] private int enemyCount;
    [SerializeField] private int asteroidsCount;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private GameObject gameOver;

    private readonly List<EnemyDirectionController> enemyControllers = new List<EnemyDirectionController>();
    
    private void Start()
    {
        GameObject player = CreatePlayer();

        CreateTrackingCamera(player);

        TorusPositionSpawner spawner = new TorusPositionSpawner(player.transform, 60, 100);

        Shooter shooter = new Shooter(projectilePrefab, player.transform, spawner, 600);
        SystemManager.RegisterSystem(new PlayerShooter(shooter));

        for (int i = 0; i < enemyCount; i++)
        {
            CreateEnemy(spawner, player.transform);
        }

        for (int i = 0; i < asteroidsCount; i++)
        {        
            Asteroid.CreateAsteroid(spawner);
        }
        GameEvents.GameOver.AddListener(OnGameOver);
    }

    private void OnGameOver(GameObject player)
    {
        player.SetActive(false); // Todo: destroy player
        gameOver.SetActive(true);
    }

    private static void CreateTrackingCamera(GameObject player)
    {
        Camera cam = FindObjectOfType<Camera>();
        SystemManager.RegisterSystem(new CameraFollower(cam.transform, player.transform));
    }

    private GameObject CreatePlayer()
    {
        GameObject go = CreateShip("Player", Layers.Player);
        SystemManager.RegisterSystem(new PlayerDirectionController(go.transform, leftKeyCode, rightKeyCode));
        SystemManager.RegisterSystem(new ShipController(go.transform, 16.0f));
        DeathListener unused = new DeathListener(go, Layers.Asteroid | Layers.Projectile | Layers.Enemy, OnPlayerDie);
        return go;
    }

    private void OnPlayerDie(GameObject player)
    {
        GameEvents.GameOver.Invoke(player);
        player.gameObject.SetActive(false);
        foreach (EnemyDirectionController enemies in enemyControllers)
        {
            enemies.Wander();
        }
    }

    private static GameObject CreateShip(string name, LayerMask layer)
    {
        NativeArray<Vector2> points = new NativeArray<Vector2>(3, Allocator.Temp)
        {
            [0] = new Vector2(-0.5f, -0.7f),
            [1] = new Vector2(0.5f, -0.7f),
            [2] = new Vector2(0.0f, 0.7f)
        };
        GameObject go = ObjectUtils.CreatePolygonalObject(name, layer, points, 0.2f);
        points.Dispose();
        return go;
    }

    private void CreateEnemy(IPositionSpawner spawner, Transform player)
    {
        GameObject enemy = CreateShip("Enemy", Layers.Enemy);
        enemy.transform.position = spawner.Position();
        EnemyDirectionController enemyDirectionController = new EnemyDirectionController(enemy.transform, player);
        enemyControllers.Add(enemyDirectionController);
        SystemManager.RegisterSystem(enemyDirectionController);
        SystemManager.RegisterSystem(new ShipController(enemy.transform, 8.0f));
        SystemManager.RegisterSystem(new Reallocator(enemy.transform, spawner));
        DeathListener unused = new DeathListener(enemy, /*Todo: Layers.Asteroid |*/ Layers.Projectile | Layers.Player, OnEnemyDie);
        SystemManager.RegisterSystem(new EnemyFieldOfView(enemy.transform, 120.0f, 20.0f, 
            transparentMaterial, 1 << Layers.Asteroid, () =>
            {
                enemyDirectionController.Seek();
                LineOfSightPredicate predicate = new LineOfSightPredicate(enemy.transform);
                Shooter shooter = new Shooter(projectilePrefab, enemy.transform, spawner, 100);
                SystemManager.RegisterSystem(new EnemyShooter(shooter, predicate));
            }));
        
    }

    private void OnEnemyDie(GameObject obj)
    {
        obj.SetActive(false); // Todo: Destroy
    }
}