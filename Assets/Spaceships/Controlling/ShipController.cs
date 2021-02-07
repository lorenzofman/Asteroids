using Systems;
using UnityEngine;

/// <summary>
/// Moves the ship forwards
/// </summary>
public readonly struct ShipController : ISystem
{
    private readonly Transform ship;
    private readonly float velocity;

    public ShipController(Transform ship, float velocity)
    {
        this.ship = ship;
        this.velocity = velocity;
    }

    public void OnUpdate()
    {
        ship.position += ship.up * velocity * Time.deltaTime;
    }

}