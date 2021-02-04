using Assets.AllyaExtension;
using UnityEngine;

internal readonly struct EnemyDirectionController : IShipController
{
    private readonly Transform transform;
    private readonly ISteeringBehaviour currentBehaviour;
    public EnemyDirectionController(Transform transform)
    {
        this.transform = transform;
        currentBehaviour = new Wander(5.0f, 10.0f, Angle.FromDegrees(1080.0f));
        Scheduler.OnGizmos.Subscribe(DebugGizmos);
    }

    private void DebugGizmos()
    {
        currentBehaviour.DebugGizmos(transform.position, transform.up);
    }

    Vector2 IShipController.Direction(Vector2 direction)
    {
        return currentBehaviour.Force(direction);
    }
}