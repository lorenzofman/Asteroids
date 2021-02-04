using UnityEngine;

public interface IPositionSpawner
{
    Vector2 Position();
    bool OutOfRange(Vector3 position);
}
