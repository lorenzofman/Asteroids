using System;
using UnityEngine;

public static class CollisionHelper
{
    private static void ListenToCollision<T>(this Component component, PhysicsEventType eventType, Action callback) where T : Component
    {
        BasePhysicsListener listener = SelectListener(component, eventType);
        listener.AddListener(typeof(T), callback);
    }

    private static BasePhysicsListener SelectListener(Component component, PhysicsEventType eventType)
    {
        return eventType switch
        {
            PhysicsEventType.Collision => component.GetOrCreate<CollisionListener>(),
            PhysicsEventType.Collision2D => component.GetOrCreate<Collision2DListener>(),
            PhysicsEventType.Trigger => component.GetOrCreate<TriggerListener>(),
            PhysicsEventType.Trigger2D => component.GetOrCreate<Trigger2DListener>(),
            _ => null
        };
    }
}