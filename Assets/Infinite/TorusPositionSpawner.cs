using Assets.AllyaExtension;
using UnityEngine;

public readonly struct TorusPositionSpawner : IPositionSpawner
{
    private readonly Transform parent;
    private readonly float innerRadius;
    private readonly float outerRadius;
    private readonly float outerRadiusSq;

    public TorusPositionSpawner(Transform parent, float innerRadius, float outerRadius)
    {
        this.parent = parent;
        this.innerRadius = innerRadius;
        this.outerRadius = outerRadius;
        outerRadiusSq = outerRadius * outerRadius;
        Scheduler.OnGizmos.Subscribe(OnDrawGizmos);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Vector3 center = parent.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(center, innerRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, outerRadius);
    }

    public Vector2 Position() => RandomUtils.RandomSpawnPosition(innerRadius, outerRadius) + parent.position;

    public bool OutOfRange(Vector3 position) => (parent.position - position).sqrMagnitude > outerRadiusSq;
}
