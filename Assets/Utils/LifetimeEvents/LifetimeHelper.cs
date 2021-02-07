using System;
using UnityEngine;

public static class LifetimeHelper
{
    public static void OnDestroy(this GameObject go, Action callback)
    {
        DestructionListener listener = go.GetOrCreate<DestructionListener>();
        listener.Initialize(callback);
    }
}