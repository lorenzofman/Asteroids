using UnityEngine;

internal interface ISteeringModifier
{
    Vector2 Modify(Vector2 force);
}