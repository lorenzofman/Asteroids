using System;
using UnityEngine;

public static class CollisionHelper
{
    public static void ListenToCollision(this GameObject gameObject, PhysicsEventType eventType, Action<GameObject> callback)
    {
        BasePhysicsListener listener = SelectListener(gameObject, eventType);
        listener.AddListener(callback);
    }

    private static BasePhysicsListener SelectListener(GameObject gameObject, PhysicsEventType eventType)
    {
        return eventType switch
        {
            PhysicsEventType.Collision => gameObject.GetOrCreate<CollisionListener>(),
            PhysicsEventType.Collision2D => gameObject.GetOrCreate<Collision2DListener>(),
            PhysicsEventType.Trigger => gameObject.GetOrCreate<TriggerListener>(),
            PhysicsEventType.Trigger2D => gameObject.GetOrCreate<Trigger2DListener>(),
            _ => null
        };
    }
}