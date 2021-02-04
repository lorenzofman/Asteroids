using Assets.AllyaExtension;
using UnityEngine;

/// <summary>
/// Controls the ship with transform
/// </summary>
/// <remarks>
/// Todo: Similar control with forces
/// </remarks>
public readonly struct ShipController
{
    private readonly Transform ship;
    private readonly IShipController shipController;
    private const float Velocity = 16.0f;

    public ShipController(Transform ship, IShipController shipController)
    {
        this.ship = ship;
        this.shipController = shipController;
    }

    public void Begin()
    {
        Scheduler.OnUpdate.Subscribe(OnUpdate);
    }
    private void OnUpdate()
    {
        Vector3 dir = shipController.Direction(ship.transform.up);
        AimAndMove(ship.transform, dir);
    }

    private static void AimAndMove(Transform t, Vector3 dir)
    {
        t.up = dir;
        t.position += dir * Velocity * Time.deltaTime;
    }

}