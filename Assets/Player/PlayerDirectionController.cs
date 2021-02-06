using Systems;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public readonly struct PlayerDirectionController : ISystem
{
    private static readonly Angle RotationPerSecond = Angle.FromDegrees(135.0f);
    private readonly KeyCode leftKeyCode;
    private readonly Transform player;
    private readonly KeyCode rightKeyCode;

    public PlayerDirectionController(Transform player, KeyCode rightKeyCode, KeyCode leftKeyCode)
    {
        this.player = player;
        this.rightKeyCode = rightKeyCode;
        this.leftKeyCode = leftKeyCode;
    }

    public void OnUpdate()
    {
        player.transform.up = ApplyRotation(player.transform.up);
    }

    private Vector2 ApplyRotation(Vector2 direction)
    {
        if (Input.GetKey(leftKeyCode))
        {
            return direction.RotateFast(-RotationPerSecond * Time.deltaTime);
        }

        return Input.GetKey(rightKeyCode) ? direction.RotateFast(RotationPerSecond * Time.deltaTime) : direction;
    }

    
}