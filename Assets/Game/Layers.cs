using UnityEngine;

public static class Layers
{
    public static LayerMask Default = LayerMask.NameToLayer("Default");
    public static LayerMask Player = LayerMask.NameToLayer("Player");
    public static LayerMask Enemy = LayerMask.NameToLayer("Enemy");
    public static LayerMask Asteroid = LayerMask.NameToLayer("Asteroid");
    public static LayerMask Projectile = LayerMask.NameToLayer("Projectile");
    public static LayerMask FieldOfView = LayerMask.NameToLayer("Field Of View");
    public static LayerMask SteeringAvoidance = LayerMask.NameToLayer("Steering Avoidance");
}
