using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static readonly UnityEvent<GameObject> GameOver = new UnityEvent<GameObject>();
}
