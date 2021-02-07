using UnityEngine;

public class Trigger2DListener : BasePhysicsListener
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnPhysicsEvent(other.gameObject);
    }
}