using Systems;
using Enemies.SteeringBehaviour;
using UnityEngine;

internal class EnemyDirectionController : ISystem
{
    private readonly Transform enemy;
    private readonly Transform player;
    private ISteeringBehaviour currentBehaviour;
    private readonly ISteeringModifier obstacleAvoidance;
    
    public EnemyDirectionController(Transform enemy, Transform player)
    {
        this.enemy = enemy;
        this.player = player;
        Wander();
        obstacleAvoidance = new CollisionAvoidance(enemy, 1 << Layers.SteeringAvoidance, 20.0f, 90.0f);
    }

    public bool IsWandering => currentBehaviour is Wander;

    public void Wander()
    {
        if (currentBehaviour is Wander)
        {
            return;
        }
        currentBehaviour = new Wander(enemy, 1.0f, 4.0f, Angle.FromDegrees(180.0f));
    }
    
    public void Pursuit(float evaderSpeed)
    {
        if (currentBehaviour is Pursuit)
        {
            return;
        }
        currentBehaviour = new Pursuit(enemy, player, evaderSpeed);
    }
    
    public void Seek()
    {
        if (currentBehaviour is Seek)
        {
            return;
        }
        currentBehaviour = new Seek(enemy, player);
    }

    public void OnUpdate()
    {
        Vector2 final = obstacleAvoidance.Modify(currentBehaviour.Force());
        enemy.transform.up = final;
    }
}