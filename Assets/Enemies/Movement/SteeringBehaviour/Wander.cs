using UnityEngine;
using Random = UnityEngine.Random;

public class Wander : ISteeringBehaviour
{
    private readonly Transform agent;
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

    public Wander(Transform agent, float radius, float distanceFromAgent, Angle maxVariationPerSecond)
    {
        this.agent = agent;
        this.radius = radius;
        this.distanceFromAgent = distanceFromAgent;
        this.maxVariationPerSecond = maxVariationPerSecond;
        localTarget = Random.insideUnitCircle;
    }

    public Vector2 Force()
    {
        Angle rotation = maxVariationPerSecond * Random.Range(-1.0f, 1.0f) * Time.deltaTime;
        localTarget = localTarget.normalized.RotateFast(rotation) * radius;
        Vector3 force = (Vector2) agent.transform.up * (distanceFromAgent + radius) + localTarget;

        return force.normalized;
    }
}