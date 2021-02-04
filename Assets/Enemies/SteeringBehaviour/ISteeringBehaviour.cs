using UnityEngine;

public interface ISteeringBehaviour
{
    Vector2 Force(Vector2 direction);
    void DebugGizmos(Vector2 agentPosition, Vector2 direction);
}