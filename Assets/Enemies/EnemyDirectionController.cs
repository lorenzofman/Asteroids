using Systems;
using UnityEngine;

internal struct EnemyDirectionController : ISystem
{
    private readonly Transform enemy;
    private readonly Transform player;
    private ISteeringBehaviour currentBehaviour;
    public EnemyDirectionController(Transform enemy, Transform player)
    {
        this.enemy = enemy;
        this.player = player;
        currentBehaviour = new Wander(enemy, 5.0f, 10.0f, Angle.FromDegrees(1080.0f));
    }

    public void OnUpdate()
    {
        
    }
}