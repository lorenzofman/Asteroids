using UnityEngine;

public class Collision2DListener : BasePhysicsListener
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        OnPhysicsEvent(other.collider.gameObject);
    }
}