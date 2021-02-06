using UnityEngine;

public class TriggerListener : BasePhysicsListener
{
    private void OnTriggerEnter(Collider other)
    {
        OnPhysicsEvent(other.gameObject);
    }
}
