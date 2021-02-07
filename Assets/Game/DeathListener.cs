using System;
using UnityEngine;

public readonly struct DeathListener
{
    private readonly GameObject gameObject;
    private readonly Action<GameObject> callback;
    private readonly int deathLayer;

    public DeathListener(GameObject gameObject, LayerMask deathLayer, Action<GameObject> callback)
    {
        this.gameObject = gameObject;
        this.callback = callback;
        this.deathLayer = deathLayer;
        gameObject.ListenToCollision(PhysicsEventType.Collision2D, OnCollide);
    }

    private void OnCollide(GameObject other)
    {
        if ((other.layer & deathLayer) > 0)
        {
            callback.Invoke(gameObject);
        }
    }
}
