using UnityEngine;

public static class BehaviourUtils
{
    public static T GetOrCreate<T>(this Component component) where T : Component
    {
        T t = component.GetComponent<T>();
        return t == null ? t.gameObject.AddComponent<T>() : t;
    }
}
