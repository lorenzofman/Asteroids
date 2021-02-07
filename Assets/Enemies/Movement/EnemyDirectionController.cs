using Systems;
using Enemies.SteeringBehaviour;
using UnityEngine;

internal class EnemyDirectionController : ISystem
{
    private readonly Transform enemy;
    private readonly Transform player;
    private ISteeringBehaviour currentBehaviour;
    
    public EnemyDirectionController(Transform enemy, Transform player)
    {
        this.enemy = enemy;
        this.player = player;
        currentBehaviour = new Wander(enemy, 5.0f, 10.0f, Angle.FromDegrees(360.0f));
    }

    public void Wander()
    {
        currentBehaviour = new Wander(enemy, 5.0f, 10.0f, Angle.FromDegrees(360.0f));
    }
    
    public void Pursuit(float evaderSpeed)
    {
        currentBehaviour = new Pursuit(enemy, player, evaderSpeed);
    }
    
    public void Seek()
    {
        currentBehaviour = new Seek(enemy, player);
    }
    

    public void OnUpdate()
    {
        Vector2 dir = currentBehaviour.Force();
        enemy.transform.up = dir;
    }
}