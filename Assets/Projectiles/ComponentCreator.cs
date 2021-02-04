using UnityEngine;

internal readonly struct ComponentCreator<T> : ICreator<T> where T : Component
{
    private readonly T prefab;

    public ComponentCreator(T prefab)
    {
        this.prefab = prefab;
    }

    public T Create()
    {
        T component = Object.Instantiate(prefab); 
        component.gameObject.SetActive(false);
        return component;
    }
}