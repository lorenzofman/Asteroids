using System;
using UnityEngine;

public class ShipPhysicsController : MonoBehaviour
{
    public KeyCode leftKeyCode;
    public KeyCode rightKeyCode;
    
    private Rigidbody2D rb;
    private const float Velocity = 5.0f;
    private const float TorqueAcceleration = 1.0f;
    private const float MaxTorque = 45.0f;
    private const float SlowdownRatio = 1.01f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    { 
        rb.velocity = Vector2.up * Velocity;
        float torque = InputTorque();
        if (Math.Sign(torque) == Math.Sign(rb.angularVelocity))
        {
            rb.angularVelocity += TorqueAcceleration;
            rb.angularVelocity = Mathf.Min(Math.Abs(rb.angularVelocity), MaxTorque) * Math.Sign(rb.angularVelocity);
        }
        else if(!Mathf.Approximately(torque, 0))
        {
            //rb.angularVelocity /= SlowdownRatio;
            rb.angularVelocity += TorqueAcceleration;
        }
    }

    private float InputTorque()
    {
        return Input.GetKeyDown(leftKeyCode) ? 
            -1 : Input.GetKeyDown(rightKeyCode) ?
                1 : 0;
    }
}
