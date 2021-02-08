using GizmosHelper;
using UnityEngine;

/// <remarks>
/// /* https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-collision-avoidance--gamedev-7777 */
/// </remarks>
internal readonly struct CollisionAvoidance : ISteeringModifier
{
    private const int Quality = 32;
    private const float MaxAppliedForcePerSecond = 10.0f;
    private const float RepulsionForce = 50.0f;

    private readonly Transform transform;
    private readonly float lookAhead;
    private readonly float fov;
    private readonly LayerMask collisionMask;
    
    public CollisionAvoidance(Transform transform, LayerMask collisionMask, float lookAhead, float fov)
    {
        this.transform = transform;
        this.collisionMask = collisionMask;
        this.lookAhead = lookAhead;
        this.fov = fov;
    }
    
    public Vector2 Modify(Vector2 current)
    {
        Vector2 position = transform.position;
        Vector2 up = transform.up;
        Vector2 right = transform.right;
        Vector2 repulse = Vector3.zero;
        float angle = -fov * 0.5f + transform.rotation.z + 90.0f;
        float step = fov / Quality;
        for (int i = 0; i < Quality + 1; i++)
        {
            Vector2 dir = Mathf.Cos(angle * Mathf.Deg2Rad) * right + Mathf.Sin(angle * Mathf.Deg2Rad) * up;
            RaycastHit2D hit = Physics2D.Raycast(position, dir, lookAhead, collisionMask);
            GizmosFromAnywhere.Color(Color.cyan);
            GizmosFromAnywhere.DrawLine(position, position + dir * lookAhead);

            if (!hit)
            {
                angle += step;
                continue;
            }
            
            GizmosFromAnywhere.Color(Color.magenta);
            GizmosFromAnywhere.DrawLine(position, position + dir * lookAhead);

            
            if (hit.distance < Mathf.Epsilon)
            {
                /* Crashed! Nothing to do*/
                return current;
            }
            
            Transform colliderTransform = hit.collider.gameObject.transform;
            Vector2 colliderPosition = colliderTransform.position;
            Vector2 avoidance = (hit.point - colliderPosition).normalized;
            avoidance *= Time.deltaTime * step * RepulsionForce * lookAhead / hit.distance;
            repulse += avoidance;
            angle += step;
        }

        return (current + repulse.normalized * MaxAppliedForcePerSecond * Time.deltaTime).normalized;
    }
    
}