using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public readonly struct UserShipDirectionController : IShipController
{
    private static readonly Angle RotationPerSecond = Angle.FromDegrees(135.0f);
    private readonly KeyCode leftKeyCode;
    private readonly KeyCode rightKeyCode;

    public UserShipDirectionController(KeyCode rightKeyCode, KeyCode leftKeyCode)
    {
        this.rightKeyCode = rightKeyCode;
        this.leftKeyCode = leftKeyCode;
    }

    Vector2 IShipController.Direction(Vector2 direction)
    {
        if (Input.GetKey(leftKeyCode))
        {
            return direction.RotateFast(-RotationPerSecond * Time.deltaTime);
        }

        return Input.GetKey(rightKeyCode) ? direction.RotateFast(RotationPerSecond * Time.deltaTime) : direction;
    }

    
}