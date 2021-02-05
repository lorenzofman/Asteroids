using UnityEngine;

namespace Asteroids
{
    public static class AsteroidUtils
    {
        private const float Impulse = 5.0f;
        
        public static void ApplyImpulse(Rigidbody2D rb)
        {
            rb.angularVelocity = 0.0f;
            rb.velocity = Vector2.zero;
            rb.AddForce(Random.insideUnitCircle * Impulse, ForceMode2D.Impulse);
        }
    }
}