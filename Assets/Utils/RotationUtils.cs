using UnityEngine;

public static class RotationUtils
{
    public static Vector2 RotateFast(this Vector2 vec, Angle angle)
    {
        float a = angle.Radians;
        return new Vector2(
            vec.x * Mathf.Cos(a) - vec.y * Mathf.Sin(a),
            vec.x * Mathf.Sin(a) + vec.y * Mathf.Cos(a));
    }
}
