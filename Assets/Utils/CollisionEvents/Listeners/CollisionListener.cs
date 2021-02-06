using UnityEngine;

public class CollisionListener : BasePhysicsListener
{
    private void OnCollisionEnter(Collision other)
    {
        OnPhysicsEvent(other.gameObject);
    }
}