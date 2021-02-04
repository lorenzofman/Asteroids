using UnityEngine;
using Random = UnityEngine.Random;

public class Wander : ISteeringBehaviour
{
    private readonly float radius;
    private readonly float distanceFromAgent;
    private readonly Angle maxVariationPerSecond;
    
    /// <summary>
    /// Relative to circle vector
    /// </summary>
    /// <remarks>
    /// Doesn't require the current player position information
    /// </remarks>
    private Vector2 localTarget;

    public Wander(float radius, float distanceFromAgent, Angle maxVariationPerSecond)
    {
        this.radius = radius;
        this.distanceFromAgent = distanceFromAgent;
        this.maxVariationPerSecond = maxVariationPerSecond;
        localTarget = Random.insideUnitCircle;
    }

    public Vector2 Force(Vector2 direction)
    {
        Angle rotation = maxVariationPerSecond * Random.Range(-1.0f, 1.0f) * Time.deltaTime;
        localTarget = localTarget.normalized.RotateFast(rotation) * radius;
        Vector3 force = direction * (distanceFromAgent + radius) + localTarget;

        return force.normalized;
    }
    
    public void DebugGizmos(Vector2 agentPosition, Vector2 direction)
    {
        Vector2 circle = agentPosition + direction * (distanceFromAgent + radius);
        Gizmos.DrawWireSphere(circle, radius);
        Gizmos.DrawWireSphere(localTarget + circle, radius * 0.4f);
    }
}