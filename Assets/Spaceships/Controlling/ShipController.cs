using Systems;
using UnityEngine;

/// <summary>
/// Moves the ship forwards
/// </summary>
public readonly struct ShipController : ISystem
{
    private const float Velocity = 16.0f;
    private readonly Transform ship;

    public ShipController(Transform ship)
    {
        this.ship = ship;
    }

    public void OnUpdate()
    {
        ship.position += ship.up * Velocity * Time.deltaTime;
    }

}