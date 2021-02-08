using System.Collections.Generic;
using System.Threading.Tasks;
using Systems;
using Assets.AllyaExtension;
using Enemies;
using Projectiles;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform projectilePrefab;
    [SerializeField] private KeyCode leftKeyCode;
    [SerializeField] private KeyCode rightKeyCode;
    [SerializeField] private int enemyCount;
    [SerializeField] private int asteroidsCount;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TextMeshProUGUI survivedTimeText;
    [SerializeField] private TextMeshProUGUI enemiesCountText;
    [SerializeField] private TextMeshProUGUI deadEnemiesCountText;

    private static bool GameOver;

    private int Enemies
    {
        get => enemies;
        set
        {
            enemies = value;
            enemiesCountText.text = enemies.ToString();
        }
    }
    private int enemies;
    
    private int DeadEnemies
    {
        get => deadEnemies;
        set
        {
            deadEnemies = value;
            deadEnemiesCountText.text = deadEnemies.ToString();
        }
    }
    private int deadEnemies;
    private float startTime;
    
    private readonly List<EnemyDirectionController> enemyControllers = new List<EnemyDirectionController>();
    private const int EnemySpawnInterval = 10;

    private void Start()
    {
        startTime = Time.time;
        
        GameObject player = CreatePlayer();

        CreateTrackingCamera(player);

        TorusPositionSpawner spawner = new TorusPositionSpawner(player.transform, 60, 100);

        Shooter shooter = new Shooter(projectilePrefab, player.transform, spawner, 200);
        SystemManager.RegisterSystem(new PlayerShooter(shooter), new ObjectBind(player));

        for (int i = 0; i < enemyCount; i++)
        {
            CreateEnemy(spawner, player.transform);
        }

        for (int i = 0; i < asteroidsCount; i++)
        {        
            Asteroid.CreateAsteroid(spawner);
        }

        AddNewEnemies(spawner, player.transform);
    }

    private void Update()
    {
        if (GameOver)
        {
            return;
        }

        int time = (int) (Time.time - startTime);
        int minutes = time / 60;
        int seconds = time % 60;
        survivedTimeText.text = $"{minutes:00}:{seconds:00}";
    }

    private async void AddNewEnemies(IPositionSpawner spawner, Transform player)
    {
        while (Application.isPlaying && !GameOver)
        {
            CreateEnemy(spawner, player);
            await Task.Delay(EnemySpawnInterval * 1000);
        }
    }

    private static void CreateTrackingCamera(GameObject player)
    {
        Camera cam = FindObjectOfType<Camera>();
        SystemManager.RegisterSystem(new CameraFollower(cam.transform, player.transform), new ObjectBind(player), 1);
    }

    private GameObject CreatePlayer()
    {
        GameObject go = CreateShip("Player", Layers.Player);
        ObjectBind playerBind = new ObjectBind(go);
        SystemManager.RegisterSystem(new PlayerDirectionController(go.transform, leftKeyCode, rightKeyCode), playerBind);
        SystemManager.RegisterSystem(new ShipController(go.transform, 16.0f), playerBind);
        DeathListener unused = new DeathListener(go, Layers.Asteroid | Layers.Projectile | Layers.Enemy, OnPlayerDie);
        return go;
    }

    private async void OnPlayerDie(GameObject player)
    {
        GameOver = true;
        Scheduler.OnUpdate.Clear();
        Destroy(player);
        gameOver.SetActive(true);
        player.gameObject.SetActive(false);
        foreach (EnemyDirectionController enemies in enemyControllers)
        {
            enemies.Wander();
        }
        GameEvents.GameOver.Invoke();
        
        while (Application.isPlaying)
        {
            if (Input.anyKeyDown)
            {
                GameOver = false;
                SceneManager.LoadScene(0);
                break;
            }
            await Task.Yield();
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
        ObjectBind enemyBind = new ObjectBind(enemy);
        SystemManager.RegisterSystem(enemyDirectionController, enemyBind);
        SystemManager.RegisterSystem(new ShipController(enemy.transform, 8.0f), enemyBind);
        SystemManager.RegisterSystem(new EnemyReallocator(enemy.transform, spawner), enemyBind);
        DeathListener unused = new DeathListener(enemy, Layers.Asteroid | Layers.Projectile | Layers.Player | Layers.Enemy,
            obj => OnEnemyCollide(obj, enemyDirectionController, spawner));
        SystemManager.RegisterSystem(new EnemyFieldOfView(enemy.transform, 90.0f, 20.0f, 
            transparentMaterial, 1 << Layers.Asteroid, () =>
            {
                enemyDirectionController.Pursuit(16.0f);
                LineOfSightPredicate predicate = new LineOfSightPredicate(enemy.transform);
                Shooter shooter = new Shooter(projectilePrefab, enemy.transform, spawner, 100);
                SystemManager.RegisterSystem(new EnemyShooter(shooter, predicate), enemyBind);
            }), enemyBind);
        Enemies++;
    }

    private void OnEnemyCollide(GameObject obj, EnemyDirectionController dirController, IPositionSpawner spawner)
    {
        /* If is seeking/pursuiting or visible*/
        if (!dirController.IsWandering || obj.GetComponent<MeshRenderer>().isVisible)
        {
            Enemies--;
            DeadEnemies++;
            Destroy(obj);
        }
        else
        {
            obj.transform.position = spawner.Position();
        }
    }
}