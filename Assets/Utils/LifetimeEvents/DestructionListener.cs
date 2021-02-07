using System;
using UnityEngine;

public class DestructionListener : MonoBehaviour
{
    private Action callback;

    public void Initialize(Action callback)
    {
        this.callback = callback;
    }

    private void OnDestroy()
    {
        Debug.Log($"Destroying {gameObject.name}");
        callback.Invoke();
    }
}