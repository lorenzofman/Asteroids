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

        Shooter shooter = new Shooter(projectilePrefab, player.transform, spawner);
        shooter.Begin();
        
        for (int i = 0; i < enemyCount; i++)
        {
            CreateEnemy(spawner);
        }

        for (int i = 0; i < asteroidsCount; i++)
        {        
            Asteroid.CreateAsteroid(spawner);
        }
    }

    private static void CreateTrackingCamera(GameObject player)
    {
        Camera cam = FindObjectOfType<Camera>();
        CameraFollower cameraFollower = new CameraFollower(cam.transform, player.transform);
        cameraFollower.Begin();
    }

    private GameObject CreatePlayer()
    {
        GameObject go = CreateShip("Player");

        /* Components */
        UserShipDirectionController shipDirectionController = new UserShipDirectionController(leftKeyCode, rightKeyCode);
        ShipController shipController = new ShipController(go.transform, shipDirectionController);

        shipController.Begin();
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

    private static void CreateEnemy(IPositionSpawner spawner)
    {
        GameObject enemy = CreateShip("Enemy");
        enemy.transform.position = spawner.Position();
        EnemyDirectionController enemyController = new EnemyDirectionController(enemy.transform);
        ShipController controller = new ShipController(enemy.transform, enemyController);
        controller.Begin();
    }
}