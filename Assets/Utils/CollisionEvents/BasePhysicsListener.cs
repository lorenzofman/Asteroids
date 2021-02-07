using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePhysicsListener : MonoBehaviour
{
    private readonly List<Action<GameObject>> triggers = new List<Action<GameObject>>();
    
    public void AddListener(Action<GameObject> action)
    {
        triggers.Add(action);
    }

    protected void OnPhysicsEvent(GameObject other)
    {
        foreach (Action<GameObject> trigger in triggers)
        {
            trigger?.Invoke(other);
        }
    }
}