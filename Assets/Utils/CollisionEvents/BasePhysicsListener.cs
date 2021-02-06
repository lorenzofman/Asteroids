using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BasePhysicsListener : MonoBehaviour
{
    private readonly Dictionary<Type, Action> triggers = new Dictionary<Type, Action>();
    
    public void AddListener(Type tag, Action action)
    {
        triggers.Add(tag, action);
    }

    protected void OnPhysicsEvent(GameObject other)
    {
        foreach (KeyValuePair<Type, Action> trigger in triggers.Where(trigger => other.GetComponent((Type) trigger.Key)))
        {
            trigger.Value?.Invoke();
        }
    }
}