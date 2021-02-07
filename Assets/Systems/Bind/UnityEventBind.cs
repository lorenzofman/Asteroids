using System;
using Systems;
using UnityEngine.Events;

internal readonly struct UnityEventBind : IBind
{
    private readonly UnityEvent unityEvent;
    
    public UnityEventBind(UnityEvent unityEvent)
    {
        this.unityEvent = unityEvent;
    }
    
    public void Bind(Action onUnbind)
    {
        unityEvent.AddListener(() => onUnbind());
    }
}