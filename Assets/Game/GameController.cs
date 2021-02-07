using Systems;
using Enemies;
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

    private void Start()
    {
        GameObject player = CreatePlayer();
        
        CreateTrackingCamera(player);

        TorusPositionSpawner spawner = new TorusPositionSpawner(player.transform, 60, 100);

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
        GameObject go = CreateShip("Player", LayerMask.NameToLayer("Player"));
        SystemManager.RegisterSystem(new PlayerDirectionController(go.transform, leftKeyCode, rightKeyCode));
        SystemManager.RegisterSystem(new ShipController(go.transform, 16.0f));
        return go;
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
        GameObject enemy = CreateShip("Enemy", LayerMask.NameToLayer("Enemy"));
        enemy.transform.position = spawner.Position();
        EnemyDirectionController enemyDirectionController = new EnemyDirectionController(enemy.transform, player);
        SystemManager.RegisterSystem(enemyDirectionController);
        SystemManager.RegisterSystem(new ShipController(enemy.transform, 12.0f));
        SystemManager.RegisterSystem(new EnemyFieldOfView(enemy.transform, 120.0f, 20.0f, 
            transparentMaterial, 1 << LayerMask.NameToLayer("Asteroid"), 
            () => enemyDirectionController.DetectPlayer()));
        SystemManager.RegisterSystem(new Reallocator(enemy.transform, spawner));
    }
}