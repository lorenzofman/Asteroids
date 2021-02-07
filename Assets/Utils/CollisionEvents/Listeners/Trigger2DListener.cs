using UnityEngine;

public class Trigger2DListener : BasePhysicsListener
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger enter in {other.gameObject}");
        OnPhysicsEvent(other.gameObject);
    }
}