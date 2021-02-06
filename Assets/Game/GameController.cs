using Systems;
using Unity.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform projectilePrefab;
    [SerializeField] private KeyCode leftKeyCode;
    [SerializeField] private KeyCode rightKeyCode;
    [SerializeField] private int enemyCount;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnOffset;
    [SerializeField] private int asteroidsCount;

    private void Start()
    {
        GameObject player = CreatePlayer();
        
        CreateTrackingCamera(player);

        TorusPositionSpawner spawner = new TorusPositionSpawner(player.transform, spawnOffset, spawnRadius);

        SystemManager.RegisterSystem(new Shooter(projectilePrefab, player.transform, spawner));

        for (int i = 0; i < enemyCount; i++)
        {
            CreateEnemy(spawner, player.transform);
        }

        for (int i = 0; i < asteroidsCount; i++)
        {        
            Asteroid.CreateAsteroid(spawner);
        }
    }

    private static void CreateTrackingCamera(GameObject player)
    {
        Camera cam = FindObjectOfType<Camera>();
        SystemManager.RegisterSystem(new CameraFollower(cam.transform, player.transform));
    }

    private GameObject CreatePlayer()
    {
        GameObject go = CreateShip("Player");

        SystemManager.RegisterSystem(new PlayerDirectionController(go.transform, leftKeyCode, rightKeyCode));
        SystemManager.RegisterSystem(new ShipController(go.transform));
        return go;
    }

    private static GameObject CreateShip(string name)
    {
        NativeArray<Vector2> points = new NativeArray<Vector2>(3, Allocator.Temp)
        {
            [0] = new Vector2(-0.5f, -0.7f),
            [1] = new Vector2(0.5f, -0.7f),
            [2] = new Vector2(0.0f, 0.7f)
        };
        GameObject go = ObjectUtils.CreatePolygonalObject(name, points, 0.2f);
        points.Dispose();
        return go;
    }

    private static void CreateEnemy(IPositionSpawner spawner, Transform player)
    {
        GameObject enemy = CreateShip("Enemy");
        enemy.transform.position = spawner.Position();
        SystemManager.RegisterSystem(new EnemyDirectionController(enemy.transform, player));
        SystemManager.RegisterSystem(new ShipController(enemy.transform));
    }
}